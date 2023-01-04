using Endpoint.Core.Models.Git;

namespace Endpoint.Core.Strategies.Common.Git
{
    public interface IGitGenerationStrategy
    {
        bool CanHandle(GitModel model);
        void Create(GitModel model);
        int Order { get; }
    }
}
