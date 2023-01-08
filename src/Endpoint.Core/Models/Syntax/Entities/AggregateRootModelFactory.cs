using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.ValueObjects;
using System.Linq;

namespace Endpoint.Core.Models.Syntax.Entities;

public class AggregateRootModelFactory : IAggregateRootModelFactory
{
    private readonly ISyntaxService _syntaxService;

    public AggregateRootModelFactory(ISyntaxService syntaxService)
    {
        _syntaxService = syntaxService;
    }

    public AggregateRootModel Create(string resource, string properties)
    {
        AggregateRootModel model = new AggregateRootModel(resource);

        var idPropertyName = _syntaxService.SyntaxModel.IdPropertyFormat == IdPropertyFormat.Short ? "Id" : $"{((Token)resource).PascalCase}Id";

        var idDotNetType = _syntaxService.SyntaxModel.IdPropertyType == IdPropertyType.Int ? "int" : "Guid";

        model.IdPropertyName = idPropertyName;

        model.IdPropertyType = idDotNetType;

        model.Properties.Add(new PropertyModel(model, "public", idDotNetType, idPropertyName, PropertyAccessorModel.GetPrivateSet, key: true));

        if (!string.IsNullOrWhiteSpace(properties))
        {
            foreach (var property in properties.Split(','))
            {
                var nameValuePair = property.Split(':');

                model.Properties.Add(new PropertyModel(model, "public", nameValuePair.ElementAt(1), nameValuePair.ElementAt(0), PropertyAccessorModel.GetPrivateSet));
            }
        }

        return model;
    }
}
