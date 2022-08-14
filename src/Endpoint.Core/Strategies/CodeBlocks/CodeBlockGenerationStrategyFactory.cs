using System;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Strategies.CodeBlocks
{
    public class CodeBlockGenerationStrategyFactory : ICodeBlockGenerationStrategyFactory
    {
        private readonly IEnumerable<ICodeBlockGenerationStrategy> _strategies;

        public CodeBlockGenerationStrategyFactory(IEnumerable<ICodeBlockGenerationStrategy> strategies)
        {
            _strategies = strategies;
        }

        public string CreateFor(dynamic model)
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

            return strategy.Create(model);
        }
    }
}
