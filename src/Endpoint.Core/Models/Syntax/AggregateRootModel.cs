using Endpoint.Core.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Models.Syntax
{
    public class AggregateRootModel : EntityModel
    {
        public List<EntityModel> Entities { get; private set; } = new List<EntityModel>();


        public string IdPropertyName { get; set; }
        public string IdPropertyType { get; set; }

        public AggregateRootModel(string name, List<ClassProperty> classProperties)
        {
            Name = name;
            Properties = classProperties;
        }

        public AggregateRootModel(string name, bool useIntIdPropertyType, bool useShortIdProperty, string properties)
        {
            Name = name;

            IdPropertyType = useIntIdPropertyType ? "int" : "Guid";

            IdPropertyName = useShortIdProperty ? "Id" : $"{((Token)name).PascalCase}Id";

            Properties.Add(new ClassProperty("public", IdPropertyType, IdPropertyName, ClassPropertyAccessor.GetPrivateSet, key: true));

            if (!string.IsNullOrWhiteSpace(properties))
            {
                foreach (var property in properties.Split(','))
                {
                    var nameValuePair = property.Split(':');

                    Properties.Add(new ClassProperty("public", nameValuePair.ElementAt(1), nameValuePair.ElementAt(0), ClassPropertyAccessor.GetPrivateSet));
                }
            }
        }

        public AggregateRootModel(string name)
        {
            Name = name;
        }

        public AggregateRootModel()
        {

        }
    }
}
