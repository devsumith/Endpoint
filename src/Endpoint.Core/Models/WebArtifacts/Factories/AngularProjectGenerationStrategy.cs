using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Models.WebArtifacts.Factories;

public class AngularProjectGenerationStrategy : WebGenerationStrategyBase<AngularProjectModel>
{
    private readonly ICommandService _commandService;
    private readonly ILogger<AngularProjectGenerationStrategy> _logger;
    public AngularProjectGenerationStrategy(
        ICommandService commandService,
        ILogger<AngularProjectGenerationStrategy> logger,
        IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override void Create(IWebGenerationStrategyFactory webGenerationStrategyFactory, AngularProjectModel model, dynamic context = null)
    {
        _commandService.Start($"ng new {model.Name}", model.Directory);
    }
}

