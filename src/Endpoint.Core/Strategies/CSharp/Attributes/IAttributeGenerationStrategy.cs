using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies.CSharp.Attributes
{
    public interface IAttributeGenerationStrategy
    {
        public bool CanHandle(AttributeModel model);
        public string[] Generate(AttributeModel model);
    }
}
