using Endpoint.Core.Models;
using Endpoint.Core.Services;
using System.IO;

namespace Endpoint.Core.Strategies.Global
{

    public interface IBicepFileGenerationStrategy
    {
        void Generate(Settings settings);
    }

    public class BicepFileGenerationStrategy: IBicepFileGenerationStrategy
    {
        private readonly IFileSystem _fileSystem;
        private readonly ITemplateLocator _templateLocator;
        
        public BicepFileGenerationStrategy(IFileSystem fileSystem, ITemplateLocator templateLocator)
        {
            _fileSystem = fileSystem;
            _templateLocator = templateLocator;
        }
        public void Generate(Settings settings)
        {
            _fileSystem.WriteAllLines($"{settings.RootDirectory}{Path.DirectorySeparatorChar}deploy{Path.DirectorySeparatorChar}main.bicep", _templateLocator.Get("BicepFile"));

        }
    }
}
