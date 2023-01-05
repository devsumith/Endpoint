namespace Endpoint.Core.Models.Git;

public interface IGitGenerationStrategy
{
    bool CanHandle(GitModel model);
    void Create(GitModel model);
    int Order { get; }
}
