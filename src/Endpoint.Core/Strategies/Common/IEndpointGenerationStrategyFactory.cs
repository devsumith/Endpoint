using Endpoint.Core.Options;

namespace Endpoint.Core.Strategies.Common;

public interface IEndpointGenerationStrategyFactory
{
    void CreateFor(CreateEndpointOptions request);
}
