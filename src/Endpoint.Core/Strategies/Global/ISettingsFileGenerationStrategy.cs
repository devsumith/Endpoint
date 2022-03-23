using Endpoint.Core.Models;
using Endpoint.Core.Options;

namespace Endpoint.Core.Strategies
{
    public interface ISettingsFileGenerationStrategy
    {
        bool? CanHandle(CreateEndpointOptions request);
        Settings Create(CreateEndpointOptions request);
    }
}
