using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax
{
    public class RouteHandlerModel
    {
        public string Name { get; private set; }
        public string Pattern { get; private set; }
        public string DbContextName { get; private set; }
        public EntityModel Entity { get; private set; }
        public RouteHandlerType Type { get; private set; }
        public List<ProducesModel> Produces { get; private set; } = new List<ProducesModel>();

        public RouteHandlerModel(string name, string pattern, string dbContextName, EntityModel entity, RouteHandlerType routeHandlerType)
        {
            Name = name;
            Pattern = pattern;
            DbContextName = dbContextName;
            Entity = entity;
            Type = routeHandlerType;
        }

    }
}
