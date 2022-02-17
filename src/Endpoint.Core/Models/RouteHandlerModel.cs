using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public class RouteHandlerModel
    {
        public string Name { get; private set; }
        public string Pattern { get; private set; }        
        public string DbContextName { get; private set; }
        public AggregateRootModel AggregateRoot { get; private set; }
        public RouteHandlerType Type { get; private set; }
        public List<ProducesModel> Produces { get; private set; } = new List<ProducesModel>();

        public RouteHandlerModel(string name, string pattern, string dbContextName, AggregateRootModel aggregateRootModel, RouteHandlerType routeHandlerType)
        {
            Name = name;
            Pattern = pattern;
            DbContextName = dbContextName;
            AggregateRoot = aggregateRootModel;
            Type = routeHandlerType;
        }

    }
}
