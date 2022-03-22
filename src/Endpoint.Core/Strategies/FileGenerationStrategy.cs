using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Strategies
{
    public class FileGenerationStrategy
    {
        private readonly IFileSystem _fileSystem;
        private readonly ITemplateLocator _templateLocator;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ISolutionNamespaceProvider _solutionNamespaceProvider;
        private readonly ILogger _logger;

        public FileGenerationStrategy(
            IFileSystem fileSystem,
            ITemplateLocator templateLocator,
            ITemplateProcessor templateProcessor,
            ISolutionNamespaceProvider solutionNamespaceProvider,
            ILogger logger
            )
        {
            _fileSystem = fileSystem ?? throw new System.ArgumentException(nameof(fileSystem));
            _templateProcessor = templateProcessor ?? throw new System.ArgumentNullException(nameof(templateProcessor));
            _templateLocator = templateLocator ?? throw new System.ArgumentNullException(nameof(templateLocator));
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _solutionNamespaceProvider = solutionNamespaceProvider ?? throw new System.ArgumentNullException(nameof(solutionNamespaceProvider));
        }

        public void Create(FileModel model)
        {
            _logger.LogInformation($"Creating {model.Name} file at {model.Path}");

            var template = _templateLocator.Get(model.Template);

            var tokens = new TokensBuilder()
                .With("SolutionNamespace",(Token)_solutionNamespaceProvider.Get(model.Directory))
                .Build();

            foreach(var token in tokens)
            {
                model.Tokens.Add(token.Key, token.Value);
            }

            var result = _templateProcessor.Process(template, model.Tokens); ;

            _fileSystem.WriteAllLines(model.Path, result);
        }
    }
}
