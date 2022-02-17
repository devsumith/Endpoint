using Endpoint.Core.Models;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Strategies.Api
{
    public interface IRouteHandlerGenerationStrategy
    {
        string[] Create(RouteHandlerModel model);
    }
    public class RouteHandlerGenerationStrategy : IRouteHandlerGenerationStrategy
    {
        public string[] Create(RouteHandlerModel model)
        {
            List<string> content = new List<string>();

            var resourceNameToken = (Token)model.AggregateRoot.Name;
            var idPropertyName = model.AggregateRoot.IdPropertyName;
            var idPropertyType = model.AggregateRoot.IdPropertyType;

            var dbContextNameToken = (Token)model.DbContextName;

            if (model.Type == RouteHandlerType.Create)
            {
                content.Add($"app.MapPost(\"/{resourceNameToken.SnakeCasePlural}\", async ({resourceNameToken.PascalCase} {resourceNameToken.CamelCase}, {dbContextNameToken.PascalCase} context) =>");
                content.Add("{".Indent(1));
                content.Add($"context.{resourceNameToken.PascalCasePlural}.Add({resourceNameToken.CamelCase});".Indent(2));
                content.Add("await context.SaveChangesAsync();".Indent(2));
                content.Add("");
                content.Add(($"return Results.Created($\"/{resourceNameToken.SnakeCasePlural}/{resourceNameToken.SnakeCasePlural}/" + "{" + $"{resourceNameToken.CamelCase}.{idPropertyName}" + "}\"," + $"{resourceNameToken.CamelCase});").Indent(2));

                content.Add("})".Indent(1));
                content.Add($".WithName(\"Create{resourceNameToken.PascalCase}\")".Indent(1));
                content.Add($".Produces<{resourceNameToken.PascalCase}>(StatusCodes.Status201Created);".Indent(1));
                return content.ToArray();
            }

            if(model.Type == RouteHandlerType.Get)
            {
                content.Add($"app.MapGet(\"/{resourceNameToken.SnakeCasePlural}\", async ({dbContextNameToken.PascalCase} context) =>");
                content.Add($"await context.{resourceNameToken.PascalCasePlural}.ToListAsync())".Indent(1));
                content.Add($".WithName(\"GetAll{resourceNameToken.PascalCasePlural}\");".Indent(1));
            }

            if(model.Type == RouteHandlerType.GetById)
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


            if(model.Type == RouteHandlerType.Update)
            {
                content.Add($"app.MapPut(\"/{resourceNameToken.SnakeCasePlural}/" + "{" + "id" + "}" + $"\", async ({idPropertyType} id, {resourceNameToken.PascalCase} input{resourceNameToken.PascalCase}, {dbContextNameToken.PascalCase} context) =>");
                content.Add("{".Indent(1));
                content.Add($"var {resourceNameToken.CamelCase} = await context.{resourceNameToken.PascalCasePlural}.FindAsync(id);".Indent(2));
                content.Add("");
                content.Add($"if ({resourceNameToken.CamelCase} is null) return Results.NotFound();".Indent(2));
                content.Add("");

                foreach(var property in model.AggregateRoot.Properties.Where(x => x.Key == false))
                {
                    content.Add($"{resourceNameToken.CamelCase}.{((Token)property.Name).PascalCase} = input{resourceNameToken.PascalCase}.{((Token)property.Name).PascalCase};".Indent(2));
                }

                content.Add("");

                content.Add("await context.SaveChangesAsync();".Indent(2));

                content.Add("return Results.NoContent();".Indent(2));


                content.Add("})".Indent(1));

                content.Add($".WithName(\"Update{resourceNameToken.PascalCase}\")".Indent(1));
                content.Add(".Produces(StatusCodes.Status204NoContent)".Indent(1));
                content.Add(".Produces(StatusCodes.Status404NotFound);".Indent(1));
            }


            if (model.Type == RouteHandlerType.Delete)
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

            return content.ToArray();
        }
    }
}
