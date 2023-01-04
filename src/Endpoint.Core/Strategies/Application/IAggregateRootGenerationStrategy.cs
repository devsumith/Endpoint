using Endpoint.Core.Models.Syntax;

namespace Endpoint.Core.Strategies.Application
{
    public interface IAggregateRootGenerationStrategy
    {
        string[] Create(AggregateRootModel model);
    }
}
