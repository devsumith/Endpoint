// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Folders;
using Endpoint.Core.Artifacts.Projects;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Artifacts.Solutions;

public class PlantUmlSolutionParserStrategy : PlantUmlParserStrategyBase<SolutionModel>
{
    public PlantUmlSolutionParserStrategy(IServiceProvider serviceProvider)
        :base(serviceProvider)
    { }
    public int Priority => int.MaxValue;

    public  override bool CanHandle(string plantUml) => plantUml.StartsWith("@startuml");

    protected override SolutionModel Create(IPlantUmlParserStrategyFactory factory, string plantUml, dynamic context = null)
    {
        var model = new SolutionModel(context.SolutionName, context.SolutionRootDirectory);

        var srcFolder = new FolderModel("", model.SolutionDirectory);

        model.Folders.Add(srcFolder);

        var plantUmlLines = plantUml.Split(Environment.NewLine).Select(x => x.Trim()).ToArray();

        var contextLevel = 0;

        var chunk = new List<string>();

        for (var i = 1; i < plantUmlLines.Length; i++)
        {
            if (contextLevel == 0 && plantUmlLines[i].StartsWith("package"))
            {
                chunk = new List<string>
                {
                    plantUmlLines[i]
                };

                i++;
            }

            chunk.Add(plantUmlLines[i]);

            if (contextLevel == 0 && plantUmlLines[i].Contains('}'))
            {
                var childModel = factory.CreateFor(string.Join(Environment.NewLine, chunk), context);

                if (childModel is ProjectModel projectModel)
                {
                    srcFolder.Projects.Add(projectModel);
                }

                chunk = new List<string>();
            }

            if (plantUmlLines[i].Contains('{'))
            {
                contextLevel++;
            }

            if (contextLevel != 0 && plantUmlLines[i].Contains('}'))
            {
                contextLevel--;
            }
        }

        return model;
    }
}

