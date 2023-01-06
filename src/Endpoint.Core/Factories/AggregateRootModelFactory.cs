using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.ValueObjects;
using System.Linq;

namespace Endpoint.Core.Factories;

public static class AggregateRootModelFactory
{
    public static AggregateRootModel Create(string resource, string properties, bool useShortIdProperty, bool useIntIdPropertyType)
    {
        AggregateRootModel model = new AggregateRootModel(resource);

        var idPropertyName = useShortIdProperty ? "Id" : $"{((Token)resource).PascalCase}Id";

        var idDotNetType = useIntIdPropertyType ? "int" : "Guid";

        model.IdPropertyName = idPropertyName;

        model.IdPropertyType = idDotNetType;

        model.Properties.Add(new PropertyModel("public", idDotNetType, idPropertyName, PropertyAccessorModel.GetPrivateSet, key: true));

        if (!string.IsNullOrWhiteSpace(properties))
        {
            foreach (var property in properties.Split(','))
            {
                var nameValuePair = property.Split(':');

                model.Properties.Add(new PropertyModel("public", nameValuePair.ElementAt(1), nameValuePair.ElementAt(0), PropertyAccessorModel.GetPrivateSet));
            }
        }

        return model;
    }
}

public static class EntityFactory
{
    public static EntityModel Create(string name, string properties)
    {
        EntityModel model = new EntityModel() {  Name = name };


        model.Properties.Add(new PropertyModel("public", "Guid", $"{((Token)name).PascalCase}Id", PropertyAccessorModel.GetPrivateSet, key: true));

        if (!string.IsNullOrWhiteSpace(properties))
        {
            foreach (var property in properties.Split(','))
            {
                var nameValuePair = property.Split(':');

                model.Properties.Add(new PropertyModel("public", nameValuePair.ElementAt(1), nameValuePair.ElementAt(0), PropertyAccessorModel.GetPrivateSet));
            }
        }

        return model;
    }
}
