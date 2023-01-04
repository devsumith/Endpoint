using Endpoint.Core.Models.Syntax;

namespace Endpoint.Core.Strategies.Api
{
    public interface IRouteHandlerGenerationStrategy
    {
        string[] Create(RouteHandlerModel model);
    }
}
