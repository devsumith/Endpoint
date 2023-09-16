// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Projects;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Artifacts.Folders;

public class FolderModel
{
    public FolderModel()
    {

    }

    public FolderModel(string name, string parentDirectory)
    {
        Name = name;
        Directory = Path.Combine(parentDirectory,name);
        SubFolders = new List<FolderModel>();
        Projects = new List<ProjectModel>();
        Files = new List<FileModel>();
    }

    public int Priority { get; set; } = 0;
    public string Name { get; set; }
    public string Directory { get; set; }
    public List<ProjectModel> Projects { get; set; }
    public List<FolderModel> SubFolders { get; set; }
    public List<FileModel> Files { get; set; }
}

