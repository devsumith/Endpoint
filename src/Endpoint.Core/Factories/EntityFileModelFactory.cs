using Endpoint.Core.Models.Artifacts;
using Endpoint.Core.Models.Syntax;
using System.Linq;

namespace Endpoint.Core.Factories
{
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


            foreach(var prop in $"Id:long,{properties}".Split(',').Distinct())
            {
                var propType = prop.Split(':')[1];
                var propName = prop.Split(':')[0];

                var classProperty = new ClassProperty("public", propType, propName, ClassPropertyAccessor.GetPrivateSet);

                entityFileModel.Entity.Properties.Add(classProperty);
            }

            return entityFileModel;
        } 
    }
}
