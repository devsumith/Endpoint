using Endpoint.Core.Models.Syntax.Attributes;

namespace Endpoint.Core.Strategies.CSharp.Attributes;

public class ProducesAttributeGenerationStrategy : IAttributeGenerationStrategy
{
    public bool CanHandle(AttributeModel model) => model.Type == AttributeType.Produces;

    public string Create(AttributeModel model) => "[Produces(MediaTypeNames.Application.Json)]";
}
