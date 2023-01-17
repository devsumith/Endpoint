namespace Endpoint.Core.Abstractions;

public interface IArtifactGenerationStrategy
{
    bool CanHandle(object model, dynamic context = null);
    void Create(object model, dynamic context = null);
    int Priority { get; }
}
