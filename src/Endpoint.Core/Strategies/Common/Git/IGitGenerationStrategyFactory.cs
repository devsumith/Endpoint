using Endpoint.Core.Models.Git;

namespace Endpoint.Core.Strategies.Common.Git
{
    public interface IGitGenerationStrategyFactory
    {
        void CreateFor(GitModel model);
    }
}
