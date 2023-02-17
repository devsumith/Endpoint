// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Artifacts.Solutions;
using Endpoint.Core.Services;
using System.IO;

namespace Endpoint.Core.Models.Syntax;

public class SyntaxService : ISyntaxService
{
    public SyntaxModel SyntaxModel { get; set; }
    public SolutionModel SolutionModel { get; set; }

    public SyntaxService(
        IPlantUmlParserStrategyFactory parserStrategyFactory,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        string directory)
    {
        var solutionPath = fileProvider.Get("*.sln", directory);

        if (solutionPath != "FileNotFound")
        {
            var solutionDirectory = Path.GetDirectoryName(solutionPath);

            var plantUmlPath = $"{solutionDirectory}{Path.DirectorySeparatorChar}documentation{Path.DirectorySeparatorChar}model.plantuml";

            if (fileSystem.Exists(plantUmlPath))
            {
                var plantUml = fileSystem.ReadAllText(plantUmlPath);

                SolutionModel = parserStrategyFactory.CreateFor(plantUml, new
                {
                    SolutionRootDirectory = Path.GetDirectoryName(solutionDirectory),
                    SolutionName = Path.GetFileNameWithoutExtension(solutionPath)
                });
            }
        }


    }

}

