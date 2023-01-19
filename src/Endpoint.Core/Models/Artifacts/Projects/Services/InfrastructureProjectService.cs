using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Models.Syntax.Classes.Factories;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Models.Artifacts.Projects.Services;

public class InfrastructureProjectService: IInfrastructureProjectService
{
    private readonly ILogger<InfrastructureProjectService> _logger;
    private readonly IClassModelFactory _classModelFactory;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly IFileModelFactory _fileModelFactory;

    public InfrastructureProjectService(
        ILogger<InfrastructureProjectService> logger,
        IClassModelFactory classModelFactory,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        IFileModelFactory fileModelFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _classModelFactory = classModelFactory ?? throw new ArgumentNullException(nameof(classModelFactory));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
    }

    public void DbContextAdd(string directory)
    {
        var dbContext = _classModelFactory.DbContext("", default, directory);

        var fileModel = _fileModelFactory.CreateCSharp(dbContext, directory);

        _artifactGenerationStrategyFactory.CreateFor(fileModel);
    }
}

