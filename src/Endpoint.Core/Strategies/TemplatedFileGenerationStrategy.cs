using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using System;

namespace Endpoint.Core.Strategies
{
    public class TemplatedFileGenerationStrategy: IFileGenerationStrategy
    {
        private readonly IFileSystem _fileSystem;
        private readonly ITemplateLocator _templateLocator;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ISolutionNamespaceProvider _solutionNamespaceProvider;
        private readonly ILogger _logger;

        public TemplatedFileGenerationStrategy(
            IFileSystem fileSystem,
            ITemplateLocator templateLocator,
            ITemplateProcessor templateProcessor,
            ISolutionNamespaceProvider solutionNamespaceProvider,
            ILogger logger
            )
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
            _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _solutionNamespaceProvider = solutionNamespaceProvider ?? throw new ArgumentNullException(nameof(solutionNamespaceProvider));
        }

        public int Order => 0;
        public bool CanHandle(FileModel model) => model is TemplatedFileModel;

        public void Create(dynamic model) => Create(model);

        private void Create(TemplatedFileModel model)
        {
            _logger.LogInformation($"Creating {model.Name} file at {model.Path}");

            var template = _templateLocator.Get(model.Template);

            var tokens = new TokensBuilder()
                .With("SolutionNamespace", (Token)_solutionNamespaceProvider.Get(model.Directory))
                .Build();

            foreach (var token in tokens)
            {
                model.Tokens.Add(token.Key, token.Value); 
            }

            var result = _templateProcessor.Process(template, model.Tokens); ;

            _fileSystem.WriteAllLines(model.Path, result);
        }
    }
}
