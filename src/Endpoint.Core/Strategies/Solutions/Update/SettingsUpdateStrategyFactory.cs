// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Strategies.Solutions.Update
{
    public class SettingsUpdateStrategyFactory : ISettingsUpdateStrategyFactory
    {
        private readonly IEnumerable<ISettingsUpdateStrategy> _strategies;

        public SettingsUpdateStrategyFactory(IEnumerable<ISettingsUpdateStrategy> settingsUpdateStrategyFactories)
        {
            _strategies = settingsUpdateStrategyFactories ?? throw new System.ArgumentNullException(nameof(settingsUpdateStrategyFactories));
        }

        public void Update(SolutionSettingsModel model, string entityName, string properties)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrEmpty(entityName))
            {
                throw new ArgumentNullException(nameof(entityName));
            }

            if (string.IsNullOrEmpty(properties))
            {
                throw new ArgumentNullException(nameof(properties));
            }

            var strategy = _strategies.Where(x => x.CanHandle(model, entityName, properties)).OrderByDescending(x => x.Order).FirstOrDefault();

            if (strategy == null)
            {
                throw new InvalidOperationException("Cannot find a strategy for generation for the type ");
            }

            strategy.Update(model, entityName, properties);
        }
    }
}

