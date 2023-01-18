using System.IO;
using System.Linq;

namespace Endpoint.Core.Services;

public class NamespaceProvider : INamespaceProvider
{
    public NamespaceProvider()
    {

    }

    public string Get(string directory, int depth = 0)
    {
        if (string.IsNullOrEmpty(directory))
            return "NamespaceNotFound";

        var parts = directory.Split(Path.DirectorySeparatorChar);

        if (parts.Length <= depth)
            return "NamespaceNotFound";

        var path = string.Join(Path.DirectorySeparatorChar, parts.Take(parts.Length - depth));

        var projectFile = Directory.GetFiles(path, "*.csproj").FirstOrDefault();

        if(!string.IsNullOrEmpty(projectFile))
        {
            var rootNamespace = Path.GetFileNameWithoutExtension(projectFile);

            if(depth > 0)
            {
                var subNamespace = string.Join('.', parts.Skip(parts.Length - depth));

                return $"{rootNamespace}.{subNamespace}";
            }

            return rootNamespace;
        }
        else
        {
            depth++;

            return Get(directory, depth);
        }

        throw new NotImplementedException();
    }
}
