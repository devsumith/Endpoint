// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Projects.Enums;

namespace Endpoint.Core.Artifacts.Projects;

public class ConsoleMicroserviceProjectModel : ProjectModel
{
    public ConsoleMicroserviceProjectModel(string name)
    {
        Name = name;
        DotNetProjectType = DotNetProjectType.Worker;
        Packages.Add(new PackageModel("Microsoft.Extensions.Hosting", "7.0.0"));
        Order = 0;
    }
}
