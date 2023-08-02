
using Endpoint.Core.Options;
using Endpoint.Core.Strategies.Common;


namespace Endpoint.Core.Generators;

public class EndpointGenerator
{
    public static void Generate(CreateEndpointOptions options, IEndpointGenerator factory)
    {
        factory.CreateFor(options);
    }
}

