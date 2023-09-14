// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Methods;
using Endpoint.Core.Syntax.Params;
using Endpoint.Core.Syntax.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Services;

public class DependencyInjectionService : IDependencyInjectionService
{
    private readonly ILogger<DependencyInjectionService> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly INamespaceProvider _namespaceProvider;

    public DependencyInjectionService(
        ILogger<DependencyInjectionService> logger,
        IFileProvider fileProvider,
        IArtifactGenerator artifactGenerator,
        IFileSystem fileSystem,
        INamespaceProvider namespaceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
    }

    public async Task Add(string interfaceName, string className, string directory, ServiceLifetime? serviceLifetime = null)
    {
        var diRegistration = $"services.AddSingleton<{interfaceName},{className}>();".Indent(2);

        await AddInternal(diRegistration, directory);
    }

    public async Task AddHosted(string hostedServiceName, string directory)
    {
        var diRegistration = $"services.AddHostedService<{hostedServiceName}>();".Indent(2);

        await AddInternal(diRegistration, directory);
    }

    private void UpdateConfigureServices(string diRegistration, string projectSuffix, string configureServicesFilePath)
    {
        var emptyServiceCollection = new StringBuilder().AppendJoin(Environment.NewLine, new string[] {
            "public static void Add" + projectSuffix + "Services(this IServiceCollection services) {",
            "}" }).ToString().Indent(1);

        var fileContent = _fileSystem.File.ReadAllText(configureServicesFilePath);

        var newContent = new StringBuilder();

        var registrationAdded = false;

        if (!fileContent.Contains(diRegistration) && !fileContent.Contains(emptyServiceCollection))
        {
            foreach (var line in fileContent.Split(Environment.NewLine))
            {
                if (line.Contains("this IServiceCollection services") && !registrationAdded && line.Contains("}"))
                {
                    newContent.AppendLine(line.Replace("}", string.Empty));

                    newContent.AppendLine(diRegistration);

                    newContent.AppendLine("}".Indent(1));

                    registrationAdded = true;
                }
                else
                {
                    newContent.AppendLine(line);
                }

                if (line.Contains("this IServiceCollection services") && !registrationAdded)
                {
                    newContent.AppendLine(diRegistration);
                    registrationAdded = true;
                }

            }
        }

        if (fileContent.Contains(emptyServiceCollection))
        {
            fileContent = fileContent.Replace(emptyServiceCollection, new StringBuilder()
                .AppendLine("public static void Add" + projectSuffix + "Services(this IServiceCollection services){".Indent(1))
                .AppendLine($"{diRegistration}")
                .Append("}".Indent(1))
                .ToString());

            newContent = new StringBuilder(fileContent);
        }

        _fileSystem.File.WriteAllText(configureServicesFilePath, newContent.ToString());
    }

    public async Task AddConfigureServices(string layer, string directory)
    {
        var classModel = new ClassModel("ConfigureServices");

        classModel.UsingDirectives.Add(new UsingModel() { Name = _namespaceProvider.Get(directory) });

        var methodParam = new ParamModel()
        {
            Type = new TypeModel("IServiceCollection"),
            Name = "services",
            ExtensionMethodParam = true
        };

        var method = new MethodModel()
        {
            Name = $"Add{layer}Services",
            ReturnType = new TypeModel("void"),
            Static = true,
            Params = new List<ParamModel>() { methodParam }
        };

        classModel.Static = true;

        classModel.Methods.Add(method);

        var classFileModel = new CodeFileModel<ClassModel>(classModel, classModel.UsingDirectives, classModel.Name, directory, ".cs")
        {
            Namespace = "Microsoft.Extensions.DependencyInjection"
        };

        await _artifactGenerator.GenerateAsync(classFileModel);
    }

    private async Task AddInternal(string diRegistration, string directory)
    {
        var path = _fileProvider.Get("ConfigureServices.cs", directory);

        var projectPath = _fileProvider.Get("*.csproj", directory);

        var projectDirectory = Path.GetDirectoryName(projectPath);

        var projectSuffix = Path.GetFileNameWithoutExtension(projectPath).Split('.').Last();

        var configureServicesFilePath = Path.Combine(projectDirectory, "ConfigureServices.cs");

        if (path == Constants.FileNotFound)
        {
            await AddConfigureServices(projectSuffix, projectDirectory);
        }

        UpdateConfigureServices(diRegistration, projectSuffix, configureServicesFilePath);
    }
}