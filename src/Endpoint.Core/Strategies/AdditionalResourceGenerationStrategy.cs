using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Strategies
{
    public interface IAdditionalResourceGenerationStrategyFactory
    {
        void CreateFor(Settings model, string resource, List<string> properties, string directory);
    }

    public interface IAdditionalResourceGenerationStrategy
    {
        bool CanHandle(Settings model);
        void Create(Settings model, string resource, List<string> properties, string directory);
    }

    public class AdditionalResourceGenerationStrategyFactory : IAdditionalResourceGenerationStrategyFactory
    {
        private readonly List<IAdditionalResourceGenerationStrategy> _strategies;

        public AdditionalResourceGenerationStrategyFactory(
            ILogger logger,
            IApplicationProjectFilesGenerationStrategy applicationFileService,
            IInfrastructureProjectFilesGenerationStrategy infrastructureFileService,
            IApiProjectFilesGenerationStrategy apiFileService)
        {
            _strategies = new List<IAdditionalResourceGenerationStrategy>()
            {
                new AdditionalResourceGenerationStrategy(applicationFileService, infrastructureFileService, apiFileService),
                new AdditionalMinimalApiResourceGenerationStrategy(logger)
            };
        }

        public void CreateFor(Settings model, string resource, List<string> properties, string directory)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var strategy = _strategies.Where(x => x.CanHandle(model)).FirstOrDefault();

            if (strategy == null)
            {
                throw new InvalidOperationException("Cannot find a strategy for generation for the type ");
            }

            strategy.Create(model,resource,properties,directory);
        }
    }

    public class AdditionalResourceGenerationStrategy : IAdditionalResourceGenerationStrategy
    {
        private readonly IApplicationProjectFilesGenerationStrategy _applicationFileService;
        private readonly IInfrastructureProjectFilesGenerationStrategy _infrastructureFileService;
        private readonly IApiProjectFilesGenerationStrategy _apiFileService;

        public AdditionalResourceGenerationStrategy(
            IApplicationProjectFilesGenerationStrategy applicationFileService,
            IInfrastructureProjectFilesGenerationStrategy infrastructureFileService,
            IApiProjectFilesGenerationStrategy apiFileService)
        {
            _applicationFileService = applicationFileService;
            _infrastructureFileService = infrastructureFileService;
            _apiFileService = apiFileService;
        }

        public bool CanHandle(Settings model) => !model.Minimal;

        public void Create(Settings model, string resource, List<string> properties, string directory)
        {
            _applicationFileService.BuildAdditionalResource(model.Resources.First(x => x.Name == resource), model);

            _infrastructureFileService.BuildAdditionalResource(resource, model);

            _apiFileService.BuildAdditionalResource(resource, model);
        }
    }

    public class AdditionalMinimalApiResourceGenerationStrategy : IAdditionalResourceGenerationStrategy
    {
        private readonly ILogger _logger;

        public AdditionalMinimalApiResourceGenerationStrategy(
            ILogger logger
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool CanHandle(Settings model) => model.Minimal;

        public void Create(Settings model, string resource, List<string> properties, string directory)
        {
            _logger.LogInformation(nameof(AdditionalMinimalApiResourceGenerationStrategy));

            Console.WriteLine("Press any key to continue...");

            Console.ReadKey();
        }
    }
}
