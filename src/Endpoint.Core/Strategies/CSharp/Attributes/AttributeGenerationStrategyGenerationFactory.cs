// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Syntax.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Strategies
{
    public class AttributeGenerationStrategyGenerationFactory : IAttributeGenerationStrategyGenerationFactory
    {
        private readonly IEnumerable<ISyntaxGenerationStrategy> _strategies;

        public AttributeGenerationStrategyGenerationFactory(IEnumerable<ISyntaxGenerationStrategy> strategies)
        {
            _strategies = strategies;
        }


        /*        private List<IAttributeGenerationStrategy> _strategies = new List<ISyntaxGenerationStrategy>()
                {
                    new ApiControllerAttributeGenerationStrategy(),
                    new AuthorizeAttributeGenerationStrategy(),
                    new HttpAttributeGenerationStrategy(),
                    new ConsumesAttributeGenerationStrategy(),
                    new HttpAttributeGenerationStrategy(),
                    new ProducesAttributeGenerationStrategy(),
                    new ProducesResponseTypeAttributeGenerationStrategy(),
                    new RouteAttributeGenerationStrategy(),
                    new SwaggerOperationAttributeGenerationStrategy(),
                };*/

        public string CreateFor(AttributeModel model)
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

