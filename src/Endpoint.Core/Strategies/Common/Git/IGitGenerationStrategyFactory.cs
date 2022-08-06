using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies.Common.Git
{
    public interface IGitGenerationStrategyFactory
    {
        void CreateFor(GitModel model);
    }
}
