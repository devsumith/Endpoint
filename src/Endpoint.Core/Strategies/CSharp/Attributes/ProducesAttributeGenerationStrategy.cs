using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies.CSharp.Attributes
{
    public class ProducesAttributeGenerationStrategy : IAttributeGenerationStrategy
    {
        public bool CanHandle(AttributeModel model) => model.Type == AttributeType.Produces;

        public string[] Create(AttributeModel model) => new string[1] { "[Produces(MediaTypeNames.Application.Json)]" };
    }
}
