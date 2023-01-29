using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Models.Artifacts.Projects.Commands;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Models.Artifacts.Projects.Strategies;

public class ApiProjectEnsureArtifactGenerationStrategy : ArtifactGenerationStrategyBase<ProjectReferenceModel>
{
    private readonly ILogger<ApiProjectEnsureArtifactGenerationStrategy> _logger;
    private readonly IFileModelFactory _fileModelFactory;
    private readonly IFileSystem _fileSystem;
    private readonly IFileProvider _fileProvider;
    private readonly ICommandService _commandService;

    public ApiProjectEnsureArtifactGenerationStrategy(
        IFileModelFactory fileModelFactory,
        IFileSystem fileSystem,
        IFileProvider fileProvider,
        IServiceProvider serviceProvider,
        ICommandService commandService,
        ILogger<ApiProjectEnsureArtifactGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem;
        _fileProvider = fileProvider;
        _commandService = commandService;
        _fileModelFactory = fileModelFactory;
    }

    public override bool CanHandle(object model, dynamic context = null)
        => model is ProjectReferenceModel && context != null && context.Command is ApiProjectEnsure;

    public override int Priority => 10;

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, ProjectReferenceModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        var projectDirectory = Path.GetDirectoryName(_fileProvider.Get("*.csproj", model.ReferenceDirectory));

        EnsureDefaultFilesRemoved(projectDirectory);

        EnsurePackagesInstalled(projectDirectory);

        EnsureProjectsReferenced(projectDirectory);

        EnsureApiDefaultFilesAdd(artifactGenerationStrategyFactory, projectDirectory);        
    }

    private void EnsureDefaultFilesRemoved(string projectDirectory)
    {
        _fileSystem.Delete($"{projectDirectory}{Path.DirectorySeparatorChar}Controllers{Path.DirectorySeparatorChar}WeatherForecastController.cs");
        _fileSystem.Delete($"{projectDirectory}{Path.DirectorySeparatorChar}WeatherForecast.cs");
    }

    private void EnsureApiDefaultFilesAdd(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, string projectDirectory)
    {
        var projectName = Path.GetFileNameWithoutExtension(projectDirectory).Split('.').First();

        var dbContext = $"{projectName}DbContext";

        if(!_fileSystem.Exists($"{projectDirectory}{Path.DirectorySeparatorChar}Properties{Path.DirectorySeparatorChar}launchSettings.json"))
        {
            artifactGenerationStrategyFactory.CreateFor(_fileModelFactory.LaunchSettingsJson(projectDirectory, projectName, 5000));
        }

        if (!_fileSystem.Exists($"{projectDirectory}{Path.DirectorySeparatorChar}ConfigureServices.cs"))
        {
            artifactGenerationStrategyFactory.CreateFor(_fileModelFactory.CreateTemplate("Api.ConfigureServices", "ConfigureServices", projectDirectory, tokens: new TokensBuilder()
                .With("DbContext", dbContext)
                .Build()));
        }


        if (!_fileSystem.Exists($"{projectDirectory}{Path.DirectorySeparatorChar}Program.cs"))
        {
            artifactGenerationStrategyFactory.CreateFor(_fileModelFactory.CreateTemplate("Api.Program", "Program", projectDirectory));
        }
    }

    private void EnsurePackagesInstalled(string projectDirectory)
    {
        var projectPath = _fileProvider.Get("*.csproj", projectDirectory);

        foreach (var package in new string[] {
            "Serilog",
            "SerilogTimings",
            "Serilog.AspNetCore",
            "Swashbuckle.AspNetCore",
            "Swashbuckle.AspNetCore.Annotations",
            "Swashbuckle.AspNetCore.Swagger",
            "Swashbuckle.AspNetCore.Newtonsoft"
        })
        {            
            var projectFileContents = _fileSystem.ReadAllText(projectPath);

            if (!projectFileContents.Contains($"PackageReference Include=\"{package}\""))
            {
                _commandService.Start($"dotnet add package {package}", projectDirectory);
            }
        }
    }

    private void EnsureProjectsReferenced(string projectDirectory)
    {
        var projectName = Path.GetFileNameWithoutExtension(projectDirectory).Split('.').First();

        _commandService.Start($"dotnet add {projectDirectory} reference \"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}BuildingBlocks{Path.DirectorySeparatorChar}Messaging{Path.DirectorySeparatorChar}Messaging.Udp{Path.DirectorySeparatorChar}Messaging.Udp.csproj{Path.DirectorySeparatorChar}\"", projectDirectory);

        _commandService.Start($"dotnet add {projectDirectory} reference \"..{Path.DirectorySeparatorChar}{projectName}.Infrastructure{Path.DirectorySeparatorChar}{projectName}.Infrastructure.csproj\"", projectDirectory);
    }
}