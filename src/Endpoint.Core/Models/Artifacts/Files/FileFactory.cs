using System.Collections.Generic;

namespace Endpoint.Core.Models.Artifacts.Files;

public class FileFactory: IFileFactory
{
    public TemplatedFileModel CreateTemplate(string template, string name, string directory, string extension = "cs", string filename = null, Dictionary<string, object> tokens = null)
    {
        return new TemplatedFileModel() { Template = template, Name = name, Directory = directory, Extension = extension, Tokens = tokens };
    }
}
