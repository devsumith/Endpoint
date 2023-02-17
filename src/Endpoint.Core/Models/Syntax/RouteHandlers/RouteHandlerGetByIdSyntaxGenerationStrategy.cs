// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Models.Syntax.RouteHandlers;

public class RouteHandlerGetByIdSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<RouteHandlerModel>
{
    private readonly ILogger<RouteHandlerGetByIdSyntaxGenerationStrategy> _logger;
    public RouteHandlerGetByIdSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<RouteHandlerGetByIdSyntaxGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override bool CanHandle(object model, dynamic context = null)
        => model is RouteHandlerModel routeHandlerModel && routeHandlerModel.Type == RouteType.GetById;

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, RouteHandlerModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0} and type {1}.", model, model.Type);

        var resource = (SyntaxToken)model.Entity.Name;

        var idPropertyName = $"{model.Entity.Name}Id";

        var idPropertyType = "guid";

        var dbContext = (SyntaxToken)model.DbContextName;

        var builder = new StringBuilder();

        builder.AppendLine($"app.MapGet(\"/{resource.SnakeCasePlural}/" + "{" + "id" + "}" + $"\", async ({idPropertyType} id, {dbContext.PascalCase} context) =>");

        builder.AppendLine($"await context.{resource.PascalCasePlural}.FindAsync(id)".Indent(1));

        builder.AppendLine($"is {resource.PascalCase} {resource.CamelCase}".Indent(2));

        builder.AppendLine($"? Results.Ok({resource.CamelCase})".Indent(3));

        builder.AppendLine(": Results.NotFound())".Indent(3));

        builder.AppendLine($".WithName(\"Get{resource.PascalCase}ById\")".Indent(1));

        builder.AppendLine($".Produces<{resource.PascalCase}>(StatusCodes.Status200OK)".Indent(1));

        builder.AppendLine(".Produces(StatusCodes.Status404NotFound);".Indent(1));

        return builder.ToString();
    }
}
