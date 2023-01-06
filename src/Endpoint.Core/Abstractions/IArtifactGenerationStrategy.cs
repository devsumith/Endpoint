namespace Endpoint.Core.Abstractions;

public interface IArtifactGenerationStrategy
{
    bool CanHandle(object model, dynamic configuration = null);
    void Create(object model, dynamic configuration = null);
    int Priority { get; }
}
