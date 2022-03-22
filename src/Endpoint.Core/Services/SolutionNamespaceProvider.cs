using System.IO;

namespace Endpoint.Core.Services
{
    public interface ISolutionNamespaceProvider
    {
        string Get(string directory);
    }
    public class SolutionNamespaceProvider : ISolutionNamespaceProvider
    {
        private readonly IFileProvider _fileProvider;

        public SolutionNamespaceProvider(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        public string Get(string directory) => Path.GetFileNameWithoutExtension(_fileProvider.Get("*.sln", directory));
    }
}
