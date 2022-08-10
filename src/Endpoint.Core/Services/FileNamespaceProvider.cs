using System;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Services
{

    public class FileNamespaceProvider: IFileNamespaceProvider
    {
        private readonly IFileProvider _fileProvider;

        public FileNamespaceProvider(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        }

        public string Get(string directory)
        {
            var projectNamespace = Path.GetFileNameWithoutExtension(_fileProvider.Get("*.csproj", directory));

            var projectDirectoryParts = Path.GetDirectoryName(_fileProvider.Get("*.csproj", directory)).Split(Path.DirectorySeparatorChar);

            var fileDirectoryParts = Path.GetDirectoryName(directory).Split(Path.DirectorySeparatorChar).Skip(projectDirectoryParts.Length);

            return fileDirectoryParts.Count() > 0 ? $"{projectNamespace}.{string.Join(".", fileDirectoryParts)}" : projectNamespace;
        }
    }
}
