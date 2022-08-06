using Endpoint.Core.Models;
using Endpoint.Core.Services;
using System.IO;

namespace Endpoint.Core.Strategies.Common
{
    public interface IGitIgnoreFileGenerationStrategy
    {
        void Generate(Settings settings);
    }

    public class GitIgnoreFileGenerationStrategy: IGitIgnoreFileGenerationStrategy
    {
        private readonly IFileSystem _fileSystem;
        private readonly ITemplateLocator _templateLocator;
        
        public GitIgnoreFileGenerationStrategy(IFileSystem fileSystem, ITemplateLocator templateLocator)
        {
            _fileSystem = fileSystem;
            _templateLocator = templateLocator;
        }
        public void Generate(Settings settings)
        {
            _fileSystem.WriteAllLines($"{settings.RootDirectory}{Path.DirectorySeparatorChar}.gitignore", _templateLocator.Get("GitIgnoreFile"));

        }
    }
}
