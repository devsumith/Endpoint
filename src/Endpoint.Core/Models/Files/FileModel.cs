using System.Collections.Generic;

namespace Endpoint.Core.Models.Files;

public class FileModel
{
    public string Name { get; init; }
    public string Directory { get; init; }
    public string Extension { get; init; }
    public string Path => $"{Directory}{System.IO.Path.DirectorySeparatorChar}{Name}.{Extension}";

    public List<string> Metadata { get; set; } = new List<string>();
    public FileModel()
    {

    }
}
