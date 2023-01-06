using Endpoint.Core.Models.Syntax.RouteHandlers;

namespace Endpoint.Core.Strategies.Api;

public interface IRouteHandlerGenerationStrategy
{
    string Create(RouteHandlerModel model);
}
