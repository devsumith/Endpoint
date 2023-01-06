﻿using Endpoint.Core.Models.Artifacts.ApiProjectModels;
using Endpoint.Core.Options;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Strategies
{
    public class AdditionalResourceGenerationStrategyFactory : IAdditionalResourceGenerationStrategyFactory
    {
        private readonly List<IAdditionalResourceGenerationStrategy> _strategies;

        public AdditionalResourceGenerationStrategyFactory(
            ILogger logger,
            IApplicationProjectFilesGenerationStrategy applicationFileService,
            IInfrastructureProjectFilesGenerationStrategy infrastructureFileService,
            IApiProjectFilesGenerationStrategy apiFileService,
            ISettingsProvider settingsProvider,
            IFileSystem fileSystem)
        {
            _strategies = new List<IAdditionalResourceGenerationStrategy>()
            {
                new AdditionalResourceGenerationStrategy(applicationFileService, infrastructureFileService, apiFileService, settingsProvider, fileSystem),
                new AdditionalMinimalApiResourceGenerationStrategy(logger)
            };
        }

        public void CreateFor(AddResourceOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var strategy = _strategies.Where(x => x.CanHandle(options)).OrderByDescending(x => x.Order).FirstOrDefault();

            if (strategy == null)
            {
                throw new InvalidOperationException("Cannot find a strategy for generation for the type ");
            }

            strategy.Create(options);
        }
    }
}