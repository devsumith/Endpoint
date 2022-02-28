using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies.CSharp.Attributes
{
    public class AuthorizeAttributeGenerationStrategy : IAttributeGenerationStrategy
    {
        public bool CanHandle(AttributeModel model) => model.Type == AttributeType.Authorize;

        public string[] Create(AttributeModel model) => new string[1] { "[Authorize]" };
    }
}
