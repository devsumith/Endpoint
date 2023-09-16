// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using System.Text;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Methods;
using Endpoint.Core.Syntax.Params;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleNLG.Extensions;

namespace Endpoint.Core.Services;

public class DependencyInjectionService : IDependencyInjectionService
{
    private readonly ILogger<DependencyInjectionService> logger;
    private readonly IFileProvider fileProvider;
    private readonly IFileSystem fileSystem;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly INamespaceProvider namespaceProvider;

    public DependencyInjectionService(
        ILogger<DependencyInjectionService> logger,
        IFileProvider fileProvider,
        IArtifactGenerator artifactGenerator,
        IFileSystem fileSystem,
        INamespaceProvider namespaceProvider)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
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

    public async Task AddConfigureServices(string layer, string directory)
    {
        var classModel = new ClassModel("ConfigureServices");

        classModel.Usings.Add(new () { Name = namespaceProvider.Get(directory) });

        var methodParam = new ParamModel()
        {
            Type = new ("IServiceCollection"),
            Name = "services",
            ExtensionMethodParam = true,
        };

        var method = new MethodModel()
        {
            Name = $"Add{layer}Services",
            ReturnType = new ("void"),
            Static = true,
            Params = new () { methodParam },
        };

        classModel.Static = true;

        classModel.Methods.Add(method);

        var classFileModel = new CodeFileModel<ClassModel>(classModel, classModel.Usings, classModel.Name, directory, Endpoint.Core.Constants.FileExtensions.CSharpFile)
        {
            Namespace = "Microsoft.Extensions.DependencyInjection",
        };

        await artifactGenerator.GenerateAsync(classFileModel);
    }

    private void UpdateConfigureServices(string diRegistration, string projectSuffix, string configureServicesFilePath)
    {
        var syntax = new string[]
        {
            "public static void Add" + projectSuffix + "Services(this IServiceCollection services)",

            "{",

            "}",
        };

        var emptyServiceCollection = new StringBuilder().AppendJoin(Environment.NewLine, syntax).ToString().Indent(1);

        var fileContent = fileSystem.File.ReadAllText(configureServicesFilePath);

        var newContent = new StringBuilder();

        var registrationAdded = false;

        var addRegistration = false;

        if (!fileContent.Contains(diRegistration))
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

                    if (addRegistration && line.trim() == "{")
                    {
                        newContent.AppendLine(diRegistration);
                        registrationAdded = true;
                        addRegistration = false;
                    }
                }

                if (line.Contains("this IServiceCollection services") && !registrationAdded)
                {
                    addRegistration = true;
                }
            }
        }

        /*
                if (fileContent.Contains(emptyServiceCollection))
                {
                    fileContent = fileContent.Replace(emptyServiceCollection, new StringBuilder()
                        .AppendLine("public static void Add" + projectSuffix + "Services(this IServiceCollection services){".Indent(1))
                        .AppendLine($"{diRegistration}")
                        .Append("}".Indent(1))
                        .ToString());

                    newContent = new StringBuilder(fileContent);
                }*/

        fileSystem.File.WriteAllText(configureServicesFilePath, newContent.ToString());
    }

    private async Task AddInternal(string diRegistration, string directory)
    {
        var path = fileProvider.Get("ConfigureServices.cs", directory);

        var projectPath = fileProvider.Get("*.csproj", directory);

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