// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;

namespace Endpoint.Core.Artifacts.Files.Factories;

public class EntityFileFactory : IEntityFileFactory
{
    private readonly ICodeAnalysisService codeAnalysisService;

    public EntityFileFactory(ICodeAnalysisService codeAnalysisService)
    {
        this.codeAnalysisService = codeAnalysisService;
    }

    public EntityFileModel Create(string name, string properties, string directory)
    {
        /*        var entity = new EntityModel(name);

                var idProperty = new StringBuilder();

                idProperty.Append(_codeAnalysisService.SyntaxModel.IdPropertyFormat == IdPropertyFormat.Long ? $"{entity.Name}Id" : "Id");

                idProperty.Append(':');

                idProperty.Append(_codeAnalysisService.SyntaxModel.IdPropertyType == IdPropertyType.Guid ? "guid" : "int");

                foreach (var prop in $"{idProperty},{properties}".Split(',').Distinct())
                {
                    var propType = prop.Split(':')[1];
                    var propName = prop.Split(':')[0];

                    var classProperty = new PropertyModel(entity, AccessModifier.Public, new TypeModel() { Name = propType }, propName, PropertyAccessorModel.GetPrivateSet);

                    entity.Properties.Add(classProperty);
                }

                var entityFileModel = new EntityFileModel(entity, directory);

                return entityFileModel;*/

        throw new NotImplementedException();
    }
}
