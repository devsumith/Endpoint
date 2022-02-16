using Endpoint.Core.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Models
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

        public AggregateRoot(string name, bool useIntIdPropertyType, bool useShortIdProperty, string properties)
        {
            Name = name;

            var idDotNetType = useIntIdPropertyType ? "int" : "Guid";

            var idPropertyName = useShortIdProperty ? "Id" : $"{((Token)name).PascalCase}Id";

            Properties.Add(new ClassProperty("public", idDotNetType, idPropertyName, ClassPropertyAccessor.GetPrivateSet, key: true));

            if (!string.IsNullOrWhiteSpace(properties))
            {
                foreach (var property in properties.Split(','))
                {
                    var nameValuePair = property.Split(':');

                    Properties.Add(new ClassProperty("public", nameValuePair.ElementAt(1), nameValuePair.ElementAt(0), ClassPropertyAccessor.GetPrivateSet));
                }
            }
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
