using Endpoint.Core.Models;
using Endpoint.Core.Options;

namespace Endpoint.Core
{
    public interface ISettingsFileGenerationStrategyFactory
    {
        Settings CreateFor(CreateEndpointOptions request);
    }
}
