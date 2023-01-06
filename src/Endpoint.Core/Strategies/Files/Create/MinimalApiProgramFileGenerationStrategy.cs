using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Api;
using Endpoint.Core.Strategies.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Strategies.Files.Create
{
    public class MinimalApiProgramFileGenerationStrategy : IFileGenerationStrategy
    {
        private readonly IFileSystem _fileSystem;
        private readonly ITemplateLocator _templateLocator;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ILogger _logger;
        private readonly IWebApplicationBuilderGenerationStrategy _webApplicationBuilderGenerationStrategy;
        private readonly IWebApplicationGenerationStrategy _webApplicationGenerationStrategy;
        private readonly ISyntaxGenerationStrategyFactory _syntaxGenerationStrategyFactory;
        public MinimalApiProgramFileGenerationStrategy(
            IFileSystem fileSystem,
            ITemplateLocator templateLocator,
            ITemplateProcessor templateProcessor,
            ILogger logger,
            IWebApplicationBuilderGenerationStrategy webApplicationBuilderGenerationStrategy,
            IWebApplicationGenerationStrategy webApplicationGenerationStrategy,
            ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory
            )
        {
            _fileSystem = fileSystem ?? throw new ArgumentException(nameof(fileSystem));
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
            _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _webApplicationBuilderGenerationStrategy = webApplicationBuilderGenerationStrategy ?? throw new ArgumentNullException(nameof(webApplicationBuilderGenerationStrategy));
            _webApplicationGenerationStrategy = webApplicationGenerationStrategy ?? throw new ArgumentNullException(nameof(webApplicationGenerationStrategy));
            _syntaxGenerationStrategyFactory = syntaxGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(syntaxGenerationStrategyFactory));
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

            content.Add(_webApplicationBuilderGenerationStrategy.Create(model.Name, model.DbContextName));

            content.Add("");

            content.Add(_webApplicationGenerationStrategy.Create(model.Name, model.DbContextName, model.RouteHandlers));

            content.Add("");

            foreach (var entity in model.Entities)
            {
                content.Add(_syntaxGenerationStrategyFactory.CreateFor(entity));

                content.Add("");
            }

            content.Add(new DbContextGenerationStrategy().Create(new DbContextModel(model.DbContextName, model.Entities)));

            _fileSystem.WriteAllText(model.Path, string.Join(Environment.NewLine, content));
        }

        public void Update(dynamic model) => Update(model);

        private void Update(MinimalApiProgramFileModel model)
        {
            _logger.LogInformation($"Updating {model.Name} file at {model.Path}");

            var existingContent = File.ReadLines(model.Path);

            var content = new List<string>();

            /* Look for var app = builder.Build(); and insert the new routeHandlers before that
             * 
             * then add the class
             * 
             * then update the dbCOntext
             * 
             */


            foreach (var line in existingContent)
            {
                content.Add(line);

                if (line.StartsWith("var app = builder.Build();"))
                {

                }
            }

            _fileSystem.WriteAllText(model.Path, string.Join(Environment.NewLine, content));
        }


    }
}
