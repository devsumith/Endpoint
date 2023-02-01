// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using Endpoint.Core.Models.Artifacts.Solutions;

namespace Endpoint.Core.Models.Artifacts.Workspaces
{
    public class WorkspaceModel
    {
        public string Name { get; set; }
        public string Directory => $"{ParentDirectory}{Path.DirectorySeparatorChar}{Name}";
        public string ParentDirectory { get; init; }
        public List<SolutionModel> Solutions { get; private set; } = new();
    }
}

