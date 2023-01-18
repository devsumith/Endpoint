using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.Types;

namespace Endpoint.Core.Models.Syntax.Classes;

public class ClassModelFactory : IClassModelFactory
{
    public ClassModel CreateEntity(string name, string properties)
    {
        var classModel = new ClassModel(name);

        var hasId = false;

        if (!string.IsNullOrEmpty(properties))
        {
            foreach (var property in properties.Split(','))
            {
                var parts = property.Split(':');
                var propName = parts[0];
                var propType = parts[1];

                if (propName == $"{name}Id")
                    hasId = true;

                classModel.Properties.Add(new PropertyModel(classModel, AccessModifier.Public, new TypeModel(propType), propName, PropertyAccessorModel.GetSet));

            }
        }

        if(!hasId)
        {
            classModel.Properties.Add(new PropertyModel(classModel, AccessModifier.Public, new("Guid"), $"{name}Id", PropertyAccessorModel.GetSet));
        }

        return classModel;
    }
}
