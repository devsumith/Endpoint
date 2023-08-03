// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.RouteHandlers;

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

    public override bool CanHandle(object model, dynamic context = null)
    {
        if (model is RouteHandlerModel routeHandlerModel)
        {
            return routeHandlerModel.Type == RouteType.Update;
        }

        return false;
    }
    public override async Task<string> CreateAsync(ISyntaxGenerator syntaxGenerator, RouteHandlerModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}
