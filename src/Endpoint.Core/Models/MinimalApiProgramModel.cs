using Endpoint.Core.ValueObjects;
using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public class MinimalApiProgramModel
    {
        public string ApiNamespace { get; set; }
        public List<string> Usings { get; private set; } = new List<string>()
        {
            "Microsoft.EntityFrameworkCore",
            "Microsoft.OpenApi.Models",
            "System.Reflection"
        };

        public  string DbContextName { get; private set; }

        public List<AggregateRootModel> AggregateRoots { get; private set; } = new List<AggregateRootModel>();

        public List<RouteHandlerModel> RouteHandlers { get; private set; } = new List<RouteHandlerModel>();

        public MinimalApiProgramModel(string apiNamespace, string dbContextName, List<AggregateRootModel> aggregateRoots)
        {
            AggregateRoots = aggregateRoots;
            ApiNamespace = apiNamespace;
            DbContextName = dbContextName;

            foreach(var aggregateRoot in aggregateRoots)
            {
                var resourceNameToken = (Token)aggregateRoot.Name;

                RouteHandlers.Add(new RouteHandlerModel(
                    $"Create{resourceNameToken.PascalCase}",
                    $"/{resourceNameToken.SnakeCasePlural}",
                    dbContextName,
                    aggregateRoot,
                    RouteHandlerType.Create
                    ));

                RouteHandlers.Add(new RouteHandlerModel(
                    $"Create{resourceNameToken.PascalCase}",
                    $"/{resourceNameToken.SnakeCasePlural}",
                    dbContextName,
                    aggregateRoot,
                    RouteHandlerType.Get
                    ));

                RouteHandlers.Add(new RouteHandlerModel(
                    $"Create{resourceNameToken.PascalCase}",
                    $"/{resourceNameToken.SnakeCasePlural}",
                    dbContextName,
                    aggregateRoot,
                    RouteHandlerType.GetById
                    ));

                RouteHandlers.Add(new RouteHandlerModel(
                    $"Create{resourceNameToken.PascalCase}",
                    $"/{resourceNameToken.SnakeCasePlural}",
                    dbContextName,
                    aggregateRoot,
                    RouteHandlerType.Update
                    ));

                RouteHandlers.Add(new RouteHandlerModel(
                    $"Create{resourceNameToken.PascalCase}",
                    $"/{resourceNameToken.SnakeCasePlural}",
                    dbContextName,
                    aggregateRoot,
                    RouteHandlerType.Delete
                    ));
            }
        }
    }
}
