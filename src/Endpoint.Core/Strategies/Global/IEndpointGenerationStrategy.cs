using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies.Global
{
    public interface IEndpointGenerationStrategy
    {
        bool CanHandle(Settings model);

        void Create(Settings model);
    }
}
