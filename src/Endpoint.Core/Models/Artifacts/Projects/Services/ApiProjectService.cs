using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Models.Syntax.Classes.Factories;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using Octokit;
using System.IO;

namespace Endpoint.Core.Models.Artifacts.Projects.Services;

public class ApiProjectService : IApiProjectService
{
    private readonly ILogger<ApiProjectService> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly IFileModelFactory _fileModelFactory;
    private readonly IClassModelFactory _classModelFactory;

    public ApiProjectService(
        ILogger<ApiProjectService> logger,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        IFileModelFactory fileModelFactory,
        IClassModelFactory classModelFactory
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
        _classModelFactory = classModelFactory ?? throw new ArgumentNullException(nameof(classModelFactory));
    }

    public void ControllerAdd(EntityModel entity, string directory)
    {
        _logger.LogInformation("Controller Add");

        var csProjPath = _fileProvider.Get("*.csproj", directory);

        var csProjDirectory = Path.GetDirectoryName(csProjPath);

        EnsureApiPresets(csProjDirectory);

        var controllersDirectory = $"{csProjDirectory}{Path.DirectorySeparatorChar}Controllers";

        _fileSystem.CreateDirectory(controllersDirectory);

        var controllerClassModel = _classModelFactory.CreateController(entity, csProjDirectory);

        _artifactGenerationStrategyFactory.CreateFor(_fileModelFactory.CreateCSharp(controllerClassModel, controllersDirectory));
    }

    public void AddApiFiles(string serviceName, string directory)
    {
        var tokens = new TokensBuilder()
            .With("ServiceName", serviceName)
            .With("DbContextName", $"{serviceName}DbContext")
            .With("Port","5001")
            .With("SslPort","5000")
            .Build();

        var configureServiceFile = _fileModelFactory.CreateTemplate("Api.ConfigureServices", "ConfigureServices", directory, "cs", tokens: tokens);

        var appSettingsFile = _fileModelFactory.CreateTemplate("Api.AppSettings", "appsettings", directory, "json", tokens: tokens);

        var launchSettingsFile = _fileModelFactory.CreateTemplate("Api.LaunchSettings", "launchsettings", directory, "json", tokens: tokens);


        foreach (var file in new FileModel[] { 
            configureServiceFile,
            appSettingsFile,
            launchSettingsFile
        })
        {
            _artifactGenerationStrategyFactory.CreateFor(file);
        }
    }


    public void EnsureApiPresets(string directory)
    {
        EnsurePackagesInstalled(directory);

        EnsureProjectsReferenced(directory);

        EnsureFiles(directory);
    }

    public void EnsureProjectsReferenced(string directory)
    {

    }

    public void EnsurePackagesInstalled(string directory)
    {

    }

    public void EnsureFiles(string directory)
    {

    }
}

