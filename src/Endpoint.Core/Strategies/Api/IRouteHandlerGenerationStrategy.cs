using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies.Api
{
    public interface IRouteHandlerGenerationStrategy
    {
        string[] Create(RouteHandlerModel model);
    }
}
