using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Models.Syntax.Entities;

public class AggregateRootModel : EntityModel
{
    public List<EntityModel> Entities { get; private set; }

    public string IdPropertyName { get; set; }
    public string IdPropertyType { get; set; }

    public AggregateRootModel(string name, List<PropertyModel> classProperties)
        :base(name)
    {
        Name = name;
        Properties = classProperties;
        Entities = new List<EntityModel>();
    }

    public AggregateRootModel(string name, bool useIntIdPropertyType, bool useShortIdProperty, string properties)
        :base(name)
    {
        Name = name;

        IdPropertyType = useIntIdPropertyType ? "int" : "Guid";

        IdPropertyName = useShortIdProperty ? "Id" : $"{((Token)name).PascalCase}Id";

        Properties.Add(new PropertyModel(this, "public", IdPropertyType, IdPropertyName, PropertyAccessorModel.GetPrivateSet, key: true));

        if (!string.IsNullOrWhiteSpace(properties))
        {
            foreach (var property in properties.Split(','))
            {
                var nameValuePair = property.Split(':');

                Properties.Add(new PropertyModel(this, "public", nameValuePair.ElementAt(1), nameValuePair.ElementAt(0), PropertyAccessorModel.GetPrivateSet));
            }
        }
    }

    public AggregateRootModel(string name)
        :base(name)
    {
        Name = name;
    }
}
