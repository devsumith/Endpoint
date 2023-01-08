using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.RouteHandlers;

public class RouteHandlerModelFactory : IRouteHandlerModelFactory
{
    public List<RouteHandlerModel> Create(string dbContextName, AggregateRootModel aggregateRoot)
    {
        var resourceNameToken = (Token)aggregateRoot.Name;

        var routes = new List<RouteHandlerModel>
        {
            new RouteHandlerModel(
            $"Create{resourceNameToken.PascalCase}",
            $"/{resourceNameToken.SnakeCasePlural}",
            dbContextName,
            aggregateRoot,
            RouteType.Create
            ),

            new RouteHandlerModel(
            $"Get{resourceNameToken.PascalCasePlural}",
            $"/{resourceNameToken.SnakeCasePlural}",
            dbContextName,
            aggregateRoot,
            RouteType.Get
            ),

            new RouteHandlerModel(
            $"Get{resourceNameToken.PascalCase}ById",
            $"/{resourceNameToken.SnakeCasePlural}",
            dbContextName,
            aggregateRoot,
            RouteType.GetById
            ),

            new RouteHandlerModel(
            $"Update{resourceNameToken.PascalCase}",
            $"/{resourceNameToken.SnakeCasePlural}",
            dbContextName,
            aggregateRoot,
            RouteType.Update
            ),

            new RouteHandlerModel(
            $"Delete{resourceNameToken.PascalCase}",
            $"/{resourceNameToken.SnakeCasePlural}",
            dbContextName,
            aggregateRoot,
            RouteType.Delete
            )
        };

        return routes;
    }
}
