using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System;

namespace Endpoint.Core.Strategies
{
    public class MinimalApiProgramFileGenerationStrategy : IFileGenerationStrategy
    {
        private readonly IFileSystem _fileSystem;
        private readonly ITemplateLocator _templateLocator;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ILogger _logger;

        public MinimalApiProgramFileGenerationStrategy(
            IFileSystem fileSystem,
            ITemplateLocator templateLocator,
            ITemplateProcessor templateProcessor,
            ILogger logger
            )
        {
            _fileSystem = fileSystem ?? throw new ArgumentException(nameof(fileSystem));
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
            _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public int Order => 0;
        public bool CanHandle(FileModel model) => model is MinimalApiProgramFileModel;

        public void Create(dynamic model) => Create(model);

        private void Create(MinimalApiProgramFileModel model)
        {
            _logger.LogInformation($"Creating {model.Name} file at {model.Path}");



            _fileSystem.WriteAllLines(model.Path, Array.Empty<string>());
        }
    }
}
