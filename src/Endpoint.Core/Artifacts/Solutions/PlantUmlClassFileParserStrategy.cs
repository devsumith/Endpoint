// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Methods;
using Endpoint.Core.Syntax.Properties;
using System.Linq;

namespace Endpoint.Core.Artifacts.Solutions;

public class PlantUmlClassFileParserStrategy : PlantUmlParserStrategyBase<ObjectFileModel<ClassModel>>
{
    public PlantUmlClassFileParserStrategy(IServiceProvider serviceProvider)
        : base(serviceProvider)
    { }

    public override bool CanHandle(string plantUml) => plantUml.StartsWith("class");

    protected override ObjectFileModel<ClassModel> Create(IPlantUmlParserStrategyFactory factory, string plantUml, dynamic context = null)
    {
        var plantUmlLines = plantUml.Split(Environment.NewLine);

        var name = plantUmlLines[0].Replace("class", string.Empty).Replace("{", string.Empty).Trim();

        var classModel = new ClassModel(name.Split('.').Last());

        for (var i = 1; i < plantUmlLines.Count(); i++)
        {
            var o = factory.CreateFor(plantUmlLines[i], new { TypeDeclarationModel = classModel });

            if (o is MethodModel method)
                classModel.Methods.Add(method);

            if (o is PropertyModel property)
                classModel.Properties.Add(property);
        }


        return new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, classModel.Name, context.Project.Directory, "cs");
    }
}

