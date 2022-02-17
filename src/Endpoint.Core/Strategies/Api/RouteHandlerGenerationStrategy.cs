using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies.Api
{
    public interface IRouteHandlerGenerationStrategy
    {
        string[] Create(RouteHandlerModel model);
    }
    public class RouteHandlerGenerationStrategy : IRouteHandlerGenerationStrategy
    {
        public string[] Create(RouteHandlerModel model)
        {
            return new string[0];
        }
    }
}
