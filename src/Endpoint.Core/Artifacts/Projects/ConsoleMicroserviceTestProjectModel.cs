// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Projects.Enums;
using Endpoint.Core.Syntax.Classes;
using System.Collections.Generic;

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
            @$"..\\{name.Replace(".Tests","")}\\{name.Replace(".Tests","")}.csproj"
        };

        var classModel = new ClassModel("Worker");

        var classFileModel = new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, "Worker", Directory, ".cs");

        Files = new List<FileModel>
        {
            classFileModel
        };
    }
}
