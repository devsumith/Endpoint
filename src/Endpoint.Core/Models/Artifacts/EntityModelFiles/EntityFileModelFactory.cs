using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Models.Syntax.Properties;
using System.Linq;

namespace Endpoint.Core.Models.Artifacts.Entities;

public static class EntityFileModelFactory
{
    public static EntityFileModel Create(string name, string properties, string directory, string @namespace)
    {
        var entityFileModel = new EntityFileModel
        {
            Entity = new EntityModel
            {
                Name = name,
                Namespace = @namespace,
            },
            Directory = directory,
            Name = name,
            Extension = "cs",
        };


        foreach (var prop in $"Id:long,{properties}".Split(',').Distinct())
        {
            var propType = prop.Split(':')[1];
            var propName = prop.Split(':')[0];

            var classProperty = new PropertyModel("public", propType, propName, PropertyAccessorModel.GetPrivateSet);

            entityFileModel.Entity.Properties.Add(classProperty);
        }

        return entityFileModel;
    }
}
