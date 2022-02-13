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
        private readonly ITemplateProcessor _templateProcessor;

        public BicepFileGenerationStrategy(IFileSystem fileSystem, ITemplateLocator templateLocator, ITemplateProcessor templateProcessor)
        {
            _fileSystem = fileSystem;
            _templateLocator = templateLocator;
            _templateProcessor = templateProcessor;
        }
        public void Generate(Settings settings)
        {
            var tokens = new TokensBuilder()
                .Build();

            var template = _templateLocator.Get("BicepFile");

            var result = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($"{settings.RootDirectory}{Path.DirectorySeparatorChar}deploy{Path.DirectorySeparatorChar}main.bicep",result);

        }
    }
}
