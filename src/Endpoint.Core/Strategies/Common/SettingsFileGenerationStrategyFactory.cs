// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Options;
using Endpoint.Core.Strategies;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core
{

    public class SolutionSettingsFileGenerator : ISolutionSettingsFileGenerator
    {
        private readonly IEnumerable<ISolutionSettingsFileGenerationStrategy> _strategies;
        private readonly ILogger _logger;
        public SolutionSettingsFileGenerator(ILogger logger, IEnumerable<ISolutionSettingsFileGenerationStrategy> strategies)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
        }

        public SolutionSettingsModel CreateFor(SolutionSettingsModel model)
        {
            var strategy = _strategies.Where(x => x.CanHandle(model).Value).FirstOrDefault();

            return strategy.Create(model);
        }
    }
}

