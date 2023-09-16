// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Projects.Enums;
using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Core.Artifacts.Projects;

public class ConsoleMicroserviceTestProjectModel : ProjectModel
{
    public ConsoleMicroserviceTestProjectModel(string name, string directory)
    {
        Name = name;
        Directory = $"{directory}{System.IO.Path.DirectorySeparatorChar}{name}";
        DotNetProjectType = DotNetProjectType.XUnit;
        Packages.Add(new PackageModel("Moq", "4.18.4"));
        Order = 1;
        References = new List<string>
        {
            @$"..\\{name.Replace(".Tests", string.Empty)}\\{name.Replace(".Tests", string.Empty)}.csproj",
        };

        var classModel = new ClassModel("Worker");

        var classFileModel = new CodeFileModel<ClassModel>(classModel, classModel.Usings, "Worker", Directory, ".cs");

        Files = new List<FileModel>
        {
            classFileModel,
        };
    }
}
