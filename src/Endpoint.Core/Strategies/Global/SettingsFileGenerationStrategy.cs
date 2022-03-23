using Endpoint.Core.Models;
using Endpoint.Core.Options;

namespace Endpoint.Core.Strategies
{
    public class SettingsFileGenerationStrategy : ISettingsFileGenerationStrategy
    {
        public bool? CanHandle(CreateEndpointOptions request)
            => !request.Minimal;
        public Settings Create(CreateEndpointOptions request)
        {
            return new();
        }
    }

    public class MinimalApiSettingsFileGenerationStrategy : ISettingsFileGenerationStrategy
    {
        public bool? CanHandle(CreateEndpointOptions request) => request.Minimal;

        public Settings Create(CreateEndpointOptions request)
        {
            return new();
        }
    }
}
