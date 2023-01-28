using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Projects.Commands;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Endpoint.Core.Models.Artifacts.Projects.Strategies;

public class ApiProjectEnsureArtifactGenerationStrategy : ArtifactGenerationStrategyBase<ProjectReferenceModel>
{
    private readonly ILogger<ApiProjectEnsureArtifactGenerationStrategy> _logger;
    private readonly IFileSystem _fileSystem;
    private readonly IFileProvider _fileProvider;
    public ApiProjectEnsureArtifactGenerationStrategy(
        IFileSystem fileSystem,
        IFileProvider fileProvider,
        IServiceProvider serviceProvider,
        ILogger<ApiProjectEnsureArtifactGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem;
        _fileProvider = fileProvider;
    }

    public override bool CanHandle(object model, dynamic context = null)
        => model is ProjectReferenceModel && context != null && context.Command is ApiProjectEnsure;

    public override int Priority => 10;

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, ProjectReferenceModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        var projectDirectory = Path.GetDirectoryName(_fileProvider.Get("*.csproj", model.ReferenceDirectory));

        EnsureDefaultFilesRemoved(projectDirectory);
        
        EnsureApiDefaultFilesAdd(projectDirectory);
        
        EnsureProjectsReferenced(projectDirectory);
        
        EnsurePackagesInstalled(projectDirectory);
    }

    private void EnsureDefaultFilesRemoved(string projectDirectory)
    {
        _fileSystem.Delete($"{projectDirectory}{Path.DirectorySeparatorChar}Controllers{Path.DirectorySeparatorChar}WeatherForecastController.cs");
        _fileSystem.Delete($"{projectDirectory}{Path.DirectorySeparatorChar}WeatherForecast.cs");
    }

    private void EnsureApiDefaultFilesAdd(string projectDirectory)
    {

    }

    private void EnsurePackagesInstalled(string projectDirectory)
    {

    }

    private void EnsureProjectsReferenced(string projectDirectory)
    {

    }
}