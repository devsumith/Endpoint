using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax
{
    public class DbContextModel
    {
        public string Name { get; set; }
        public List<EntityModel> Entities { get; private set; } = new List<EntityModel>();

        public DbContextModel(string name, List<EntityModel> entities)
        {
            Name = name;
            Entities = entities;
        }
    }
}
