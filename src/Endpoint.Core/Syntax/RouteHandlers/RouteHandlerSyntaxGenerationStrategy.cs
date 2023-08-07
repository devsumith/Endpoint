// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Syntax;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Syntax.RouteHandlers;

public class RouteHandlerSyntaxGenerationStrategy : ISyntaxGenerationStrategy<RouteHandlerModel>
{
    private readonly ILogger<RouteHandlerSyntaxGenerationStrategy> _logger;
    public RouteHandlerSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<RouteHandlerSyntaxGenerationStrategy> logger)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int Priority { get; } = 0;

    public async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, RouteHandlerModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        List<string> content = new List<string>();

        var resourceNameToken = (SyntaxToken)model.Entity.Name;
        var idPropertyName = $"{model.Entity.Name}Id";
        var idPropertyType = "guid";

        var dbContextNameToken = (SyntaxToken)model.DbContextName;

        if (model.Type == RouteType.Create)
        {
            content.Add($"app.MapPost(\"/{resourceNameToken.SnakeCasePlural}\", async ({resourceNameToken.PascalCase} {resourceNameToken.CamelCase}, {dbContextNameToken.PascalCase} context) =>");
            content.Add("{".Indent(1));
            content.Add($"context.{resourceNameToken.PascalCasePlural}.Add({resourceNameToken.CamelCase});".Indent(2));
            content.Add("await context.SaveChangesAsync();".Indent(2));
            content.Add("");
            content.Add(($"return Results.Created($\"/{resourceNameToken.SnakeCasePlural}/" + "{" + $"{resourceNameToken.CamelCase}.{idPropertyName}" + "}\"," + $"{resourceNameToken.CamelCase});").Indent(2));

            content.Add("})".Indent(1));
            content.Add($".WithName(\"Create{resourceNameToken.PascalCase}\")".Indent(1));
            content.Add($".Produces<{resourceNameToken.PascalCase}>(StatusCodes.Status201Created);".Indent(1));
            return string.Join(Environment.NewLine, content);
        }

        if (model.Type == RouteType.Get)
        {
            content.Add($"app.MapGet(\"/{resourceNameToken.SnakeCasePlural}\", async ({dbContextNameToken.PascalCase} context) =>");
            content.Add($"await context.{resourceNameToken.PascalCasePlural}.ToListAsync())".Indent(1));
            content.Add($".WithName(\"GetAll{resourceNameToken.PascalCasePlural}\");".Indent(1));
        }

        if (model.Type == RouteType.GetById)
        {
            content.Add($"app.MapGet(\"/{resourceNameToken.SnakeCasePlural}/" + "{" + "id" + "}" + $"\", async ({idPropertyType} id, {dbContextNameToken.PascalCase} context) =>");
            content.Add($"await context.{resourceNameToken.PascalCasePlural}.FindAsync(id)".Indent(1));
            content.Add($"is {resourceNameToken.PascalCase} {resourceNameToken.CamelCase}".Indent(2));
            content.Add($"? Results.Ok({resourceNameToken.CamelCase})".Indent(3));
            content.Add(": Results.NotFound())".Indent(3));
            content.Add($".WithName(\"Get{resourceNameToken.PascalCase}ById\")".Indent(1));
            content.Add($".Produces<{resourceNameToken.PascalCase}>(StatusCodes.Status200OK)".Indent(1));
            content.Add(".Produces(StatusCodes.Status404NotFound);".Indent(1));
        }


        if (model.Type == RouteType.Update)
        {
            content.Add($"app.MapPut(\"/{resourceNameToken.SnakeCasePlural}/" + "{" + "id" + "}" + $"\", async ({idPropertyType} id, {resourceNameToken.PascalCase} input{resourceNameToken.PascalCase}, {dbContextNameToken.PascalCase} context) =>");
            content.Add("{".Indent(1));
            content.Add($"var {resourceNameToken.CamelCase} = await context.{resourceNameToken.PascalCasePlural}.FindAsync(id);".Indent(2));
            content.Add("");
            content.Add($"if ({resourceNameToken.CamelCase} is null) return Results.NotFound();".Indent(2));
            content.Add("");

            foreach (var property in model.Entity.Properties.Where(x => x.Id == false))
            {
                content.Add($"{resourceNameToken.CamelCase}.{((SyntaxToken)property.Name).PascalCase} = input{resourceNameToken.PascalCase}.{((SyntaxToken)property.Name).PascalCase};".Indent(2));
            }

            content.Add("");

            content.Add("await context.SaveChangesAsync();".Indent(2));

            content.Add("return Results.NoContent();".Indent(2));


            content.Add("})".Indent(1));

            content.Add($".WithName(\"Update{resourceNameToken.PascalCase}\")".Indent(1));
            content.Add(".Produces(StatusCodes.Status204NoContent)".Indent(1));
            content.Add(".Produces(StatusCodes.Status404NotFound);".Indent(1));
        }


        if (model.Type == RouteType.Delete)
        {
            content.Add($"app.MapDelete(\"/{resourceNameToken.SnakeCasePlural}/" + "{" + "id" + "}" + $"\", async ({idPropertyType} id, {dbContextNameToken.PascalCase} context) =>");
            content.Add("{".Indent(1));
            content.Add($"if (await context.{resourceNameToken.PascalCasePlural}.FindAsync(id) is {resourceNameToken.PascalCase} {resourceNameToken.CamelCase})".Indent(2));
            content.Add("{".Indent(2));
            content.Add($"context.{resourceNameToken.PascalCasePlural}.Remove({resourceNameToken.CamelCase});".Indent(3));
            content.Add("await context.SaveChangesAsync();".Indent(3));
            content.Add($"return Results.Ok({resourceNameToken.CamelCase});".Indent(3));
            content.Add("}".Indent(2));
            content.Add("");
            content.Add("return Results.NotFound();".Indent(2));
            content.Add("})".Indent(1));
            content.Add($".WithName(\"Delete{resourceNameToken.PascalCase}\")".Indent(1));
            content.Add(".Produces(StatusCodes.Status204NoContent)".Indent(1));
            content.Add(".Produces(StatusCodes.Status404NotFound);".Indent(1));
        }

        return builder.ToString();
    }
}
