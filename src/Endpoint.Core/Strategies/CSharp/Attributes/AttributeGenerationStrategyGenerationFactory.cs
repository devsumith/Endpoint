using Endpoint.Core.Models;
using Endpoint.Core.Strategies.CSharp.Attributes;
using System.Collections.Generic;

namespace Endpoint.Core.Strategies
{
    public interface IAttributeGenerationStrategyGenerationFactory
    {
        IAttributeGenerationStrategy CreateFor(AttributeModel model);
    }

    public class AttributeGenerationStrategyGenerationFactory : IAttributeGenerationStrategyGenerationFactory
    {
        private List<IAttributeGenerationStrategy> _strategies = new List<IAttributeGenerationStrategy>();

        public AttributeGenerationStrategyGenerationFactory()
        {

        }

        public IAttributeGenerationStrategy CreateFor(AttributeModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
