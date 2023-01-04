using Endpoint.Core.Models.Git;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Strategies.Common.Git
{
    public class GitGenerationStrategyFactory : IGitGenerationStrategyFactory
    {
        private readonly IEnumerable<IGitGenerationStrategy> _strategies;

        public GitGenerationStrategyFactory(IEnumerable<IGitGenerationStrategy> strategies)
        {
            _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
        }

        public void CreateFor(GitModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var strategy = _strategies.Where(x => x.CanHandle(model)).OrderByDescending(x => x.Order).FirstOrDefault();

            if (strategy == null)
            {
                throw new InvalidOperationException("Cannot find a strategy for generation for the type ");
            }

            strategy.Create(model);
        }
    }
}
