using Endpoint.Core.Models.Syntax.Attributes;

namespace Endpoint.Core.Strategies
{
    public interface IAttributeGenerationStrategyGenerationFactory
    {
        string CreateFor(AttributeModel model);
    }
}
