using Endpoint.Core.Models.Syntax;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;

namespace Endpoint.Core.Factories
{
    public static class RouteHandlerFactory
    {
        public static List<RouteHandlerModel> Create(string dbContextName, AggregateRootModel aggregateRoot)
        {
            var resourceNameToken = (Token)aggregateRoot.Name;

            var routes = new List<RouteHandlerModel>();

            routes.Add(new RouteHandlerModel(
                $"Create{resourceNameToken.PascalCase}",
                $"/{resourceNameToken.SnakeCasePlural}",
                dbContextName,
                aggregateRoot,
                RouteHandlerType.Create
                ));

            routes.Add(new RouteHandlerModel(
                $"Get{resourceNameToken.PascalCasePlural}",
                $"/{resourceNameToken.SnakeCasePlural}",
                dbContextName,
                aggregateRoot,
                RouteHandlerType.Get
                ));

            routes.Add(new RouteHandlerModel(
                $"Get{resourceNameToken.PascalCase}ById",
                $"/{resourceNameToken.SnakeCasePlural}",
                dbContextName,
                aggregateRoot,
                RouteHandlerType.GetById
                ));

            routes.Add(new RouteHandlerModel(
                $"Update{resourceNameToken.PascalCase}",
                $"/{resourceNameToken.SnakeCasePlural}",
                dbContextName,
                aggregateRoot,
                RouteHandlerType.Update
                ));

            routes.Add(new RouteHandlerModel(
                $"Delete{resourceNameToken.PascalCase}",
                $"/{resourceNameToken.SnakeCasePlural}",
                dbContextName,
                aggregateRoot,
                RouteHandlerType.Delete
                ));

            return routes;
        }
    }
}
