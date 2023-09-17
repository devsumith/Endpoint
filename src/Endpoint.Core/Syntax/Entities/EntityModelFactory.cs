// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;

namespace Endpoint.Core.Syntax.Entities;

public class EntityFactory : IEntityFactory
{
    private readonly ICodeAnalysisService codeAnalysisService;

    public EntityFactory(ICodeAnalysisService codeAnalysisService)
    {
        this.codeAnalysisService = codeAnalysisService;
    }

    public EntityModel Create(string name, string properties)
    {
        /*        EntityModel model = new EntityModel(name);

                var idPropertyName = _codeAnalysisService.SyntaxModel.IdPropertyFormat == IdPropertyFormat.Short ? "Id" : $"{((SyntaxToken)name).PascalCase}Id";

                var idDotNetType = _codeAnalysisService.SyntaxModel.IdPropertyType == IdPropertyType.Int ? "int" : "Guid";

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
