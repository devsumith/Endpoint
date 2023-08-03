// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;

namespace Endpoint.Core.Artifacts.Workspaces;

public class AngularWorkspaceModel
{
    public AngularWorkspaceModel(string name, string version, string rootDirectory)
    {
        Name = name;
        Version = version;
        RootDirectory = rootDirectory;
        Directory = $"{RootDirectory}{Path.DirectorySeparatorChar}{name}";
    }
    public string Version { get; set; }
    public string Name { get; set; }
    public string RootDirectory { get; set; }
    public string Directory { get; set; }
}
