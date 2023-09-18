// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Artifacts.Files;

public class FileModel : ArtifactModel
{
    public FileModel()
    {
    }

    public FileModel(string name, string directory, string extension)
    {
        Name = name;
        Directory = directory;
        Extension = extension;
        Path = System.IO.Path.Combine(Directory, $"{Name}{Extension}");
    }

    public string Body { get; set; }

    public string Name { get; set; }

    public string Directory { get; set; }

    public string Extension { get; set; }

    public string Path { get; set; }
}
