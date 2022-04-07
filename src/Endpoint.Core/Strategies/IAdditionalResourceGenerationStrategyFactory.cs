using Endpoint.Core.Options;

namespace Endpoint.Core.Strategies
{
    public interface IAdditionalResourceGenerationStrategyFactory
    {
        void CreateFor(AddResourceOptions options);
    }
}
