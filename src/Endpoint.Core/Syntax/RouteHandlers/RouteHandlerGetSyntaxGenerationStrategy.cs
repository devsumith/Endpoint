// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.RouteHandlers;

public class RouteHandlerGetSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<RouteHandlerModel>
{
    private readonly ILogger<RouteHandlerSyntaxGenerationStrategy> _logger;
    public RouteHandlerGetSyntaxGenerationStrategy(

        ILogger<RouteHandlerSyntaxGenerationStrategy> logger)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int GetPriority() => 0;

    public bool CanHandle(object model)
        => model is RouteHandlerModel routeHandlerModel && routeHandlerModel.Type == RouteType.Get;

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, RouteHandlerModel model)
    {
        _logger.LogInformation("Generating syntax for {0} and type {1}.", model, model.Type);

        var resource = (SyntaxToken)model.Entity.Name;

        var dbContext = (SyntaxToken)model.DbContextName;

        var builder = new StringBuilder();

        builder.AppendLine($"app.MapGet(\"/{resource.SnakeCasePlural}\", async ({dbContext.PascalCase} context) =>");

        builder.AppendLine($"await context.{resource.PascalCasePlural}.ToListAsync())".Indent(1));

        builder.AppendLine($".WithName(\"GetAll{resource.PascalCasePlural}\");".Indent(1));

        return builder.ToString();
    }
}
