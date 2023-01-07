using System.Collections.Generic;

namespace Endpoint.Core.Models.Artifacts.Files;

public interface IFileFactory
{
    TemplatedFileModel CreateTemplate(string template, string name, string directory, string extension = "cs", string filename = null, Dictionary<string, object> tokens = null);
}
