using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies.Application
{
    public interface IAggregateRootGenerationStrategy
    {
        string[] Create(AggregateRootModel model);
    }
}
