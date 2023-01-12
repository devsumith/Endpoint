using System.IO;
using System.Linq;

namespace Endpoint.Core.Services;

public class FileProvider : IFileProvider
{
    public string Get(string searchPattern, string directory, int depth = 0)
    {
        var parts = directory.Split(Path.DirectorySeparatorChar);

        if (parts.Length == depth)
            return "FileNotFound";

        var file = Directory.GetFiles(string.Join(Path.DirectorySeparatorChar, parts.Take(parts.Length - depth)), searchPattern).FirstOrDefault();

        return file ?? Get(searchPattern, directory, depth + 1);

    }
}
