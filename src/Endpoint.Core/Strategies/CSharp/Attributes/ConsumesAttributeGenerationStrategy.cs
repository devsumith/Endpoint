using Endpoint.Core.Models.Syntax;

namespace Endpoint.Core.Strategies.CSharp.Attributes;

public class ConsumesAttributeGenerationStrategy : IAttributeGenerationStrategy
{
    public bool CanHandle(AttributeModel model) => model.Type == AttributeType.Consumes;

    public string Create(AttributeModel model) => "[Consumes(MediaTypeNames.Application.Json)]";
}
