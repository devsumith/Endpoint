using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies.CSharp.Attributes
{
    public class AuthorizeAttributeGenerationStrategy : IAttributeGenerationStrategy
    {
        public bool CanHandle(AttributeModel model) => model.Type == AttributeType.Authorize;

        public string[] Generate(AttributeModel model) => new string[1] { "[Authorize]" };
    }
}
