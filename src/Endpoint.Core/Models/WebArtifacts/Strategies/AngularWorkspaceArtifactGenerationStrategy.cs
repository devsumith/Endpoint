using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Models.WebArtifacts.Strategies;

public class AngularWorkspaceArtifactGenerationStrategy : ArtifactGenerationStrategyBase<AngularWorkspaceModel>
{
    private readonly ILogger<AngularWorkspaceArtifactGenerationStrategy> _logger;
    private readonly ICommandService _commandService;

    public AngularWorkspaceArtifactGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<AngularWorkspaceArtifactGenerationStrategy> logger,
        ICommandService commandService) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, AngularWorkspaceModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        _commandService.Start($"ng new {model.Name} --no-create-application", model.RootDirectory);

    }
}