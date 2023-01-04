using Endpoint.Core.Models.Syntax;
using System.Collections.Generic;

namespace Endpoint.Core.ViewModels
{
    public class EntityViewModel
    {
        public List<string> Usings { get; set; } = new List<string>();
        public List<string> Properties { get; set; } = new List<string>();
        public string Namespace { get; set; }
        public string Name { get; set; }
    }


    public static class EntityExtensions
    {

        public static EntityViewModel ToViewModel(this EntityModel entity)
        {
            var model = new EntityViewModel
            {
                Usings = entity.Usings,
                Namespace = entity.Namespace,
                Name = entity.Name
            };

            foreach (var prop in entity.Properties)
            {
                if (ClassPropertyAccessor.IsGetPrivateSet(prop.Accessors))
                {
                    model.Properties.Add($"{prop.AccessModifier} {prop.Type} {prop.Name} " + "{ get; private set; }" + (!entity.Properties.IsLast(prop) ? System.Environment.NewLine : string.Empty));
                }
            }
            return model;
        }
    }
}
