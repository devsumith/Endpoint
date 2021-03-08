using System.Collections.Generic;

namespace Endpoint.Cli.Services
{
    public interface INamespaceProvider
    {
        string GetFileNamespace(string path, List<string> fileNamespaceParts = default);
    }
}