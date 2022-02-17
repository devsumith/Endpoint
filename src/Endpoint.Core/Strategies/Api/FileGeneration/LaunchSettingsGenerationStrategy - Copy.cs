using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using System.IO;

namespace Endpoint.Core.Strategies.Api.FileGeneration
{
    public interface IMinimalAppSettingsGenerationStrategy
    {
        void Create(Settings settings);
    }
    public class MinimalAppSettingsGenerationStrategy : IMinimalAppSettingsGenerationStrategy
    {
        private readonly ITemplateProcessor _templateProcessor;
        private readonly IFileSystem _fileSystem;
        private readonly ITemplateLocator _templateLocator;

        public MinimalAppSettingsGenerationStrategy(ITemplateProcessor templateProcessor, IFileSystem fileSystem, ITemplateLocator templateLocator)
        {
            _templateProcessor = templateProcessor;
            _fileSystem = fileSystem;
            _templateLocator = templateLocator;
        }

        public void Create(Settings model)
        {
            var template = _templateLocator.Get("MinimalAppSettings");

            var tokens = new TokensBuilder()
                .With(nameof(model.DbContextName), (Token)model.DbContextName)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{model.ApiDirectory}{Path.DirectorySeparatorChar}appSettings.json", contents);
        }
    }
}
