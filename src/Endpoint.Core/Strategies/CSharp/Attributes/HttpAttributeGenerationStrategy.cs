using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies.CSharp.Attributes
{
    public class HttpAttributeGenerationStrategy : IAttributeGenerationStrategy
    {
        public bool CanHandle(AttributeModel model) => model.Type == AttributeType.Http;

        public string[] Create(AttributeModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
