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
                //RouteHandlers.Add(new RouteHandlerModel())
            }
        }
    }
}
