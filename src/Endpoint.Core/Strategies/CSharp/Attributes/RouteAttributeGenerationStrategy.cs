using Endpoint.Core.Models.Syntax.Attributes;

namespace Endpoint.Core.Strategies.CSharp.Attributes
{
    public class RouteAttributeGenerationStrategy : IAttributeGenerationStrategy
    {
        public bool CanHandle(AttributeModel model) => model.Type == AttributeType.Route;

        public string Create(AttributeModel model) => "[Route(\"api/[controller]\")]";
    }
}
