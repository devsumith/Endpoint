using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Models.Syntax.RouteHandlers;

public class RouteHandlerUpdateSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<RouteHandlerModel>
{
    private readonly ILogger<RouteHandlerSyntaxGenerationStrategy> _logger;
    public RouteHandlerUpdateSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<RouteHandlerSyntaxGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override bool CanHandle(object model, dynamic configuration = null)
    {
        if (model is RouteHandlerModel routeHandlerModel)
        {
            return routeHandlerModel.Type == RouteType.Update;
        }

        return false;
    }
    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, RouteHandlerModel model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}