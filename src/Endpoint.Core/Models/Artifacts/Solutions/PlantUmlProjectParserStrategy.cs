// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Projects;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public class PlantUmlProjectParserStrategy : PlantUmlParserStrategyBase<ProjectModel>
{
    public PlantUmlProjectParserStrategy(IServiceProvider serviceProvider)
        : base(serviceProvider)
    { }

    public override bool CanHandle(string plantUml) => plantUml.StartsWith("package");

    protected override ProjectModel Create(IPlantUmlParserStrategyFactory factory, string plantUml, dynamic context = null)
    {
        var plantUmlLines = plantUml.Split(Environment.NewLine);

        var name = plantUmlLines.First().Replace("{", null).Trim().Split(' ').Last();

        var directory = $"{context.SolutionRootDirectory}{Path.DirectorySeparatorChar}{context.SolutionName}{Path.DirectorySeparatorChar}{name}";

        var model = new ProjectModel() { Name = name, Directory = directory };

        var chunk = new List<string>();

        var contextLevel = 0;

        for (var i = 1; i < plantUmlLines.Length; i++)
        {

            if (contextLevel == 0 && (plantUmlLines[i].StartsWith("class") || plantUmlLines[i].StartsWith("interface")))
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
                var childModel = factory.CreateFor(string.Join(Environment.NewLine, chunk), new
                {
                    Project = model
                });

                if (childModel is FileModel fileModel)
                {
                    model.Files.Add(fileModel);
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

