using System.Collections.Generic;

namespace Endpoint.Core.Models.Artifacts.Files;

public class FileModel
{
    public string Name { get; init; }
    public string Directory { get; init; }
    public string Extension { get; init; }
    public string Path => $"{Directory}{System.IO.Path.DirectorySeparatorChar}{Name}.{Extension}";

    public List<string> Metadata { get; set; } = new List<string>();
    public FileModel(string name, string directory, string extension)
    {
        Name = name;
        Directory = directory;
        Extension = extension;
    }
}
