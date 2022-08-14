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

        public List<Entity> Entities { get; private set; } = new List<Entity>();

        public List<RouteHandlerModel> RouteHandlers { get; private set; } = new List<RouteHandlerModel>();

        public MinimalApiProgramModel(string apiNamespace, string dbContextName, List<Entity> entities)
        {
            Entities = entities;
            ApiNamespace = apiNamespace;
            DbContextName = dbContextName;

            foreach(var entity in entities)
            {
                var resourceNameToken = (Token)entity.Name;

                RouteHandlers.Add(new RouteHandlerModel(
                    $"Create{resourceNameToken.PascalCase}",
                    $"/{resourceNameToken.SnakeCasePlural}",
                    dbContextName,
                    entity,
                    RouteHandlerType.Create
                    ));

                RouteHandlers.Add(new RouteHandlerModel(
                    $"Create{resourceNameToken.PascalCase}",
                    $"/{resourceNameToken.SnakeCasePlural}",
                    dbContextName,
                    entity,
                    RouteHandlerType.Get
                    ));

                RouteHandlers.Add(new RouteHandlerModel(
                    $"Create{resourceNameToken.PascalCase}",
                    $"/{resourceNameToken.SnakeCasePlural}",
                    dbContextName,
                    entity,
                    RouteHandlerType.GetById
                    ));

                RouteHandlers.Add(new RouteHandlerModel(
                    $"Create{resourceNameToken.PascalCase}",
                    $"/{resourceNameToken.SnakeCasePlural}",
                    dbContextName,
                    entity,
                    RouteHandlerType.Update
                    ));

                RouteHandlers.Add(new RouteHandlerModel(
                    $"Create{resourceNameToken.PascalCase}",
                    $"/{resourceNameToken.SnakeCasePlural}",
                    dbContextName,
                    entity,
                    RouteHandlerType.Delete
                    ));
            }
        }
    }
}
