using Endpoint.Core.Models;
using Endpoint.Core.Strategies.CSharp.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Strategies
{

    public class AttributeGenerationStrategyGenerationFactory : IAttributeGenerationStrategyGenerationFactory
    {
        private List<IAttributeGenerationStrategy> _strategies = new List<IAttributeGenerationStrategy>()
        {
            new AuthorizeAttributeGenerationStrategy()
        };

        public string[] CreateFor(AttributeModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var strategy = _strategies.Where(x => x.CanHandle(model)).FirstOrDefault();

            if (strategy == null)
            {
                throw new InvalidOperationException("Cannot find a strategy for generation for the type " + model.Type);
            }

            return strategy.Create(model);
        }
    }
}
