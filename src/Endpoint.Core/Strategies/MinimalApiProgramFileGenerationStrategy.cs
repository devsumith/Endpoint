using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Api;
using Endpoint.Core.Strategies.Application;
using Endpoint.Core.Strategies.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Endpoint.Core.Strategies
{
    public class MinimalApiProgramFileGenerationStrategy : IFileGenerationStrategy
    {
        private readonly IFileSystem _fileSystem;
        private readonly ITemplateLocator _templateLocator;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ILogger _logger;
        private readonly IWebApplicationBuilderGenerationStrategy _webApplicationBuilderGenerationStrategy;
        private readonly IWebApplicationGenerationStrategy _webApplicationGenerationStrategy;

        public MinimalApiProgramFileGenerationStrategy(
            IFileSystem fileSystem,
            ITemplateLocator templateLocator,
            ITemplateProcessor templateProcessor,
            ILogger logger,
            IWebApplicationBuilderGenerationStrategy webApplicationBuilderGenerationStrategy,
            IWebApplicationGenerationStrategy webApplicationGenerationStrategy
            )
        {
            _fileSystem = fileSystem ?? throw new ArgumentException(nameof(fileSystem));
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
            _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _webApplicationBuilderGenerationStrategy = webApplicationBuilderGenerationStrategy ?? throw new ArgumentNullException(nameof(webApplicationBuilderGenerationStrategy));
            _webApplicationGenerationStrategy = webApplicationGenerationStrategy ?? throw new ArgumentNullException(nameof(webApplicationGenerationStrategy));
        }

        public int Order => 0;
        public bool CanHandle(FileModel model) => model is MinimalApiProgramFileModel;

        public void Create(dynamic model) => Create(model);

        private void Create(MinimalApiProgramFileModel model)
        {
            _logger.LogInformation($"Creating {model.Name} file at {model.Path}");

            var content = new List<string>();

            foreach (var @using in model.Usings)
            {
                content.Add($"using {@using};");
            }

            content.Add("");

            content.AddRange(_webApplicationBuilderGenerationStrategy.Create(model.Name, model.DbContextName));

            content.Add("");

            content.AddRange(_webApplicationGenerationStrategy.Create(model.Name, model.DbContextName, model.RouteHandlers));

            content.Add("");

            foreach (var aggregate in model.Aggregates)
            {
                content.AddRange(new AggregateRootGenerationStrategy().Create(aggregate));

                content.Add("");
            }

            content.AddRange(new DbContextGenerationStrategy().Create(new DbContextModel(model.DbContextName, model.Aggregates)));

            _fileSystem.WriteAllLines(model.Path, content.ToArray());
        }
    }
}
