// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Syntax.Entities;

public class EntityFactory : IEntityFactory
{
    private readonly ISyntaxService _syntaxService;

    public EntityFactory(ISyntaxService syntaxService)
    {
        _syntaxService = syntaxService;
    }

    public EntityModel Create(string name, string properties)
    {
        /*        EntityModel model = new EntityModel(name);

                var idPropertyName = _syntaxService.SyntaxModel.IdPropertyFormat == IdPropertyFormat.Short ? "Id" : $"{((SyntaxToken)name).PascalCase}Id";

                var idDotNetType = _syntaxService.SyntaxModel.IdPropertyType == IdPropertyType.Int ? "int" : "Guid";

                model.Properties.Add(new PropertyModel(model, AccessModifier.Public, new TypeModel() { Name = idDotNetType }, idPropertyName, PropertyAccessorModel.GetPrivateSet, key: true));

                if (!string.IsNullOrWhiteSpace(properties))
                {
                    foreach (var property in properties.Split(','))
                    {
                        var nameValuePair = property.Split(':');

                        model.Properties.Add(new PropertyModel(model, AccessModifier.Public, new TypeModel() { Name = nameValuePair.ElementAt(1) }, nameValuePair.ElementAt(0), PropertyAccessorModel.GetPrivateSet));
                    }
                }

                return model;*/

        throw new NotImplementedException();
    }
}

