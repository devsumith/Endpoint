using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies
{
    public interface IAttributeGenerationStrategyGenerationFactory
    {
        string[] CreateFor(AttributeModel model);
    }
}
