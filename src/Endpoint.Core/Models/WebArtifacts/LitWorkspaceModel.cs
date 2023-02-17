// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;

namespace Endpoint.Core.Models.WebArtifacts;

public class LitWorkspaceModel
{
    public LitWorkspaceModel(string name, string rootDirectory)
    {
        Name = name;
        RootDirectory = rootDirectory;
        Directory = $"{rootDirectory}{Path.DirectorySeparatorChar}{name}";
    }

    public string Name { get; set; }
    public string RootDirectory { get; set; }
    public string Directory { get; set; }
}

