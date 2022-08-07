using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public class Entity
    {
        public string AggregateRootName { get; private set; }
        public List<ClassProperty> Properties { get; private set; } = new List<ClassProperty>();
        public string Name { get; set; }

        public Entity(string aggregateRootName, string name, List<ClassProperty> classProperties)
        {
            AggregateRootName = aggregateRootName;
            Name = name;
            Properties = classProperties;
        }

        public Entity(string name, List<ClassProperty> classProperties)
        {
            Name = name;
            Properties = classProperties;
        }

        public Entity(string name)
        {
            Name = name;
        }

        public Entity()
        {

        }

        public List<string> Usings { get; set; }
        public string Namespace { get; set; }
    }
}
