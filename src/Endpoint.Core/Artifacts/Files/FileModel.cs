// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.Artifacts.Files;

public class FileModel
{
    public FileModel(string name, string directory, string extension)
    {
        Name = name;
        Directory = directory;
        Extension = extension;
    }

    public string Copyright { get; set; }
    public string Name { get; init; }
    public string Directory { get; init; }
    public string Extension { get; init; }
    public string Path => $"{Directory}{System.IO.Path.DirectorySeparatorChar}{Name}.{Extension}";
    public List<string> Metadata { get; set; } = new List<string>();
}

