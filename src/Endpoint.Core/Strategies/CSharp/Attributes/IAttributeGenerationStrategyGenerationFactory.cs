using Endpoint.Core.Models.Syntax;

namespace Endpoint.Core.Strategies
{
    public interface IAttributeGenerationStrategyGenerationFactory
    {
        string CreateFor(AttributeModel model);
    }
}
