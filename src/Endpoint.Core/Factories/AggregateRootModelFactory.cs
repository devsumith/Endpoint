using Endpoint.Core.Models;
using Endpoint.Core.ValueObjects;
using System.Linq;

namespace Endpoint.Core.Factories
{
    public static class AggregateRootModelFactory
    {
        public static AggregateRootModel Create(string resource, string properties, bool useShortIdProperty, bool useIntIdPropertyType)
        {
            AggregateRootModel model = new AggregateRootModel(resource);

            var idPropertyName = useShortIdProperty ? "Id" : $"{((Token)resource).PascalCase}Id";

            var idDotNetType = useIntIdPropertyType ? "int" : "Guid";

            model.IdPropertyName = idPropertyName;

            model.IdPropertyType = idDotNetType;

            model.Properties.Add(new ClassProperty("public", idDotNetType, idPropertyName, ClassPropertyAccessor.GetPrivateSet, key: true));

            if (!string.IsNullOrWhiteSpace(properties))
            {
                foreach (var property in properties.Split(','))
                {
                    var nameValuePair = property.Split(':');

                    model.Properties.Add(new ClassProperty("public", nameValuePair.ElementAt(1), nameValuePair.ElementAt(0), ClassPropertyAccessor.GetPrivateSet));
                }
            }

            return model;
        }
    }

    public static class EntityFactory
    {
        public static Entity Create(string name, string properties)
        {
            Entity model = new Entity() {  Name = name };


            model.Properties.Add(new ClassProperty("public", "Guid", $"{((Token)name).PascalCase}Id", ClassPropertyAccessor.GetPrivateSet, key: true));

            if (!string.IsNullOrWhiteSpace(properties))
            {
                foreach (var property in properties.Split(','))
                {
                    var nameValuePair = property.Split(':');

                    model.Properties.Add(new ClassProperty("public", nameValuePair.ElementAt(1), nameValuePair.ElementAt(0), ClassPropertyAccessor.GetPrivateSet));
                }
            }

            return model;
        }
    }
}
