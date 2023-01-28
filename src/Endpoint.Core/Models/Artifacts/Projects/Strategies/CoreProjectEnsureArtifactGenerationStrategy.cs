using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Projects.Commands;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Models.Artifacts.Projects.Strategies;

public class CoreProjectEnsureArtifactGenerationStrategy : ArtifactGenerationStrategyBase<ProjectReferenceModel>
{
    private readonly ILogger<CoreProjectEnsureArtifactGenerationStrategy> _logger;
    public CoreProjectEnsureArtifactGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<CoreProjectEnsureArtifactGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override bool CanHandle(object model, dynamic context = null)
        => model is ProjectReferenceModel && context != null && context.Command is CoreProjectEnsure;

    public override int Priority => 10;

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, ProjectReferenceModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

    }
}