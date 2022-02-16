using Endpoint.Core.Models;
using Endpoint.Core.Strategies.Global;

namespace Endpoint.Core.Generators
{
    public class EndpointGenerator
    {
        public static void Generate(Settings settings, IEndpointGenerationStrategyFactory factory)
        {
            factory.CreateFor(settings);
        }
    }
}
