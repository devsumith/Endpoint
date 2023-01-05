namespace Endpoint.Core.Models.Git;

public interface IGitGenerationStrategyFactory
{
    void CreateFor(GitModel model);
}
