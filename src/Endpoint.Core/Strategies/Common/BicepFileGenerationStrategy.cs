using Endpoint.Core.Models.Options;
using Endpoint.Core.Services;
using System.IO;

namespace Endpoint.Core.Strategies.Common
{

    public interface IBicepFileGenerationStrategy
    {
        void Generate(SettingsModel settings);
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
        public void Generate(SettingsModel settings)
        {
            _fileSystem.WriteAllLines($"{settings.RootDirectory}{Path.DirectorySeparatorChar}deploy{Path.DirectorySeparatorChar}main.bicep", _templateLocator.Get("BicepFile"));

        }
    }
}
