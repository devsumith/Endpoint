using Endpoint.Core.Options;

namespace Endpoint.Core.Strategies
{
    public interface IAdditionalResourceGenerationStrategy
    {
        int Order { get; }
        bool CanHandle(AddResourceOptions options);
        void Create(AddResourceOptions options);
    }
}
