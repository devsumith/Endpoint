using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies.CSharp.Attributes
{
    public class RouteAttributeGenerationStrategy : IAttributeGenerationStrategy
    {
        public bool CanHandle(AttributeModel model) => model.Type == AttributeType.Route;

        public string[] Create(AttributeModel model) => new string[1] { "[Route(\"api/[controller]\")]" };
    }
}
