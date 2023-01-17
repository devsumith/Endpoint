using Endpoint.Core.Abstractions;
using Endpoint.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Models.Syntax.RouteHandlers;

public class RouteHandlerDeleteSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<RouteHandlerModel>
{
    private readonly ILogger<RouteHandlerDeleteSyntaxGenerationStrategy> _logger;
    public RouteHandlerDeleteSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<RouteHandlerDeleteSyntaxGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override bool CanHandle(object model, dynamic context = null)
        => model is RouteHandlerModel routeHandlerModel && routeHandlerModel.Type == RouteType.Delete;

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, RouteHandlerModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0} and type {1}.", model, model.Type);

        var resource = (Token)model.Entity.Name;

        var idPropertyName = $"{model.Entity.Name}Id";

        var dbContext = (Token)model.DbContextName;

        var idPropertyType = "guid";

        var builder = new StringBuilder();

        builder.AppendLine($"app.MapDelete(\"/{resource.SnakeCasePlural}/" + "{" + "id" + "}" + $"\", async ({idPropertyType} id, {dbContext.PascalCase} context) =>");
        
        builder.AppendLine("{".Indent(1));
        
        builder.AppendLine($"if (await context.{resource.PascalCasePlural}.FindAsync(id) is {resource.PascalCase} {resource.CamelCase})".Indent(2));
        
        builder.AppendLine("{".Indent(2));
        
        builder.AppendLine($"context.{resource.PascalCasePlural}.Remove({resource.CamelCase});".Indent(3));
        
        builder.AppendLine("await context.SaveChangesAsync();".Indent(3));
        
        builder.AppendLine($"return Results.Ok({resource.CamelCase});".Indent(3));
        
        builder.AppendLine("}".Indent(2));
        
        builder.AppendLine("");
        
        builder.AppendLine("return Results.NotFound();".Indent(2));
        
        builder.AppendLine("})".Indent(1));
        
        builder.AppendLine($".WithName(\"Delete{resource.PascalCase}\")".Indent(1));
        
        builder.AppendLine(".Produces(StatusCodes.Status204NoContent)".Indent(1));        
        
        builder.AppendLine(".Produces(StatusCodes.Status404NotFound);".Indent(1));

        return builder.ToString();
    }
}