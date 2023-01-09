using Endpoint.Core.Enums;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.Types;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Models.Artifacts.Entities;

public class EntityFileModelFactory: IEntityFileModelFactory
{
    private readonly ISyntaxService _syntaxService;

    public EntityFileModelFactory(ISyntaxService syntaxService)
    {
        _syntaxService = syntaxService;
    }

    public EntityFileModel Create(string name, string properties, string directory)
    {
        var entity = new EntityModel(name);

        var idProperty = new StringBuilder();

        idProperty.Append(_syntaxService.SyntaxModel.IdPropertyFormat == IdPropertyFormat.Long ? $"{entity.Name}Id" : "Id");
        
        idProperty.Append(':');
        
        idProperty.Append(_syntaxService.SyntaxModel.IdPropertyType == IdPropertyType.Guid ? "guid" : "int");

        foreach (var prop in $"{idProperty},{properties}".Split(',').Distinct())
        {
            var propType = prop.Split(':')[1];
            var propName = prop.Split(':')[0];

            var classProperty = new PropertyModel(entity, AccessModifier.Public, new TypeModel() { Name = propType }, propName, PropertyAccessorModel.GetPrivateSet);

            entity.Properties.Add(classProperty);
        }

        var entityFileModel = new EntityFileModel(entity, directory);

        return entityFileModel;
    }
}
