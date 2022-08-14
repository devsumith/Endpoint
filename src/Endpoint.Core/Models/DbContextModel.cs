using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public  class DbContextModel
    {
        public string Name { get; set; }
        public List<Entity> Entities { get; private set; } = new List<Entity>();

        public DbContextModel(string name, List<Entity> entities)
        {
            Name = name;
            Entities = entities;
        }
    }
}
