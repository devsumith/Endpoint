using Endpoint.Core.Options;
using Endpoint.Core.Strategies.Global;

namespace Endpoint.Core.Generators
{
    public class EndpointGenerator
    {
        public static void Generate(CreateEndpointOptions request, IEndpointGenerationStrategyFactory factory)
        {
            factory.CreateFor(request);
        }
    }
}
