using Endpoint.Core.Models;
using Endpoint.Core.Options;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core
{

    public class SettingsFileGenerationStrategyFactory: ISettingsFileGenerationStrategyFactory
    {
        private readonly List<ISettingsFileGenerationStrategy> _strategies;
        private readonly ILogger _logger;
        public SettingsFileGenerationStrategyFactory(ILogger logger, IFileSystem fileSystem)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            

            _strategies = new List<ISettingsFileGenerationStrategy>()
            {
                new SettingsFileGenerationStrategy(fileSystem),
                new MinimalApiSettingsFileGenerationStrategy()
            };
        }

        public Settings CreateFor(Settings model)
        {
            var strategy = _strategies.Where(x => x.CanHandle(model).Value).FirstOrDefault();

            return strategy.Create(model);
        }
    }
}
