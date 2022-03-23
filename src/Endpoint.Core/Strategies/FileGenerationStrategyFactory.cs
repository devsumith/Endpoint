﻿using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Strategies
{

    public class FileGenerationStrategyFactory: IFileGenerationStrategyFactory
    {
        private readonly List<IFileGenerationStrategy> _strategies;

        public FileGenerationStrategyFactory(
            IFileSystem fileSystem,
            ITemplateLocator templateLocator,
            ITemplateProcessor templateProcessor,
            ISolutionNamespaceProvider solutionNamespaceProvider,
            ILogger logger
            )
        {
            _strategies = new List<IFileGenerationStrategy>()
            {
                new TemplatedFileGenerationStrategy(fileSystem,templateLocator,templateProcessor,solutionNamespaceProvider,logger),
                new MinimalApiProgramFileGenerationStrategy(fileSystem,templateLocator,templateProcessor,logger)
            };
        }

        public void CreateFor(FileModel model)
        {
            var strategy = _strategies.Where(x => x.CanHandle(model)).OrderByDescending(x => x.Order).FirstOrDefault();

            strategy.Create(model);
        }
    }
}
