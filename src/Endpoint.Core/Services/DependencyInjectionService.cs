using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Services;

public class DependencyInjectionService: IDependencyInjectionService
{
    private readonly ILogger<DependencyInjectionService> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;

    public DependencyInjectionService(
        ILogger<DependencyInjectionService> logger, 
        IFileProvider fileProvider,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        IFileSystem fileSystem)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
    }

    public void Add(string interfaceName, string className, string directory, ServiceLifetime? serviceLifetime = null)
    {
        var diRegistration = $"services.AddSingleton<{interfaceName},{className}>();".Indent(2);

        AddInternal(diRegistration, directory);
    }

    public void AddHosted(string hostedServiceName, string directory)
    {
        var diRegistration = $"services.AddHostedService<{hostedServiceName}>();".Indent(2);

        AddInternal(diRegistration, directory);
    }

    private void UpdateConfigureServices(string diRegistration, string projectSuffix, string configureServicesFilePath)
    {
        var emptyServiceCollection = new StringBuilder().AppendJoin(Environment.NewLine, new string[] { 
            "public static void Add" + projectSuffix + "Services(this IServiceCollection services) {", 
            "}" }).ToString().Indent(1);

        var fileContent = _fileSystem.ReadAllText(configureServicesFilePath);

        var newContent = new StringBuilder();

        var registrationAdded = false;

        if (!fileContent.Contains(diRegistration) && !fileContent.Contains(emptyServiceCollection))
        {
            foreach (var line in fileContent.Split(Environment.NewLine))
            {
                if (line.Contains("this IServiceCollection services") && !registrationAdded && line.Contains("}")) {

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

        _fileSystem.WriteAllText(configureServicesFilePath, newContent.ToString());
    }
    
    private void AddConfigureServices(string projectSuffix, string projectDirectory)
    {
        var classModel = new ClassModel("ConfigureServices");

        var methodParam = new ParamModel()
        {
            Type = new TypeModel("IServiceCollection"),
            Name = "services",
            ExtensionMethodParam = true
        };

        var method = new MethodModel()
        {
            Name = $"Add{projectSuffix}Services",
            ReturnType = new TypeModel("void"),
            Static = true,
            Params = new List<ParamModel>() { methodParam }
        };

        classModel.Static = true;

        classModel.Methods.Add(method);

        var classFileModel = new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, classModel.Name, projectDirectory, "cs")
        {
            Namespace = "Microsoft.Extensions.DependencyInjection"
        };

        _artifactGenerationStrategyFactory.CreateFor(classFileModel);
    }

    private void AddInternal(string diRegistration, string directory)
    {
        var path = _fileProvider.Get("ConfigureServices.cs", directory);

        var projectPath = _fileProvider.Get("*.csproj", directory);

        var projectDirectory = Path.GetDirectoryName(projectPath);

        var projectSuffix = Path.GetFileNameWithoutExtension(projectPath).Split('.').Last();

        var configureServicesFilePath = Path.Combine(projectDirectory, "ConfigureServices.cs");

        if (path == "FileNotFound")
        {

            AddConfigureServices(projectSuffix, projectDirectory);
        }

        UpdateConfigureServices(diRegistration, projectSuffix, configureServicesFilePath);
    }
}

