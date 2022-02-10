using System.Collections.Generic;

namespace Endpoint.SharedKernal.Models
{
    public class AggregateRoot
    {
        public List<ClassProperty> Properties { get; private set; } = new List<ClassProperty>();

        public List<Entity> Entities { get; private set; } = new List<Entity>();

        public string Name { get; set; }

        public AggregateRoot(string name, List<ClassProperty> classProperties)
        {
            Name = name;
            Properties = classProperties;
        }

        public AggregateRoot(string name)
        {
            Name = name;
        }

        public AggregateRoot()
        {

        }
    }
}
