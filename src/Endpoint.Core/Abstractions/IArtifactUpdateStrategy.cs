namespace Endpoint.Core.Abstractions;

public interface IArtifactUpdateStrategy
{
    int Priority { get; }
    bool CanHandle(dynamic context = null, params object[] args);

    void Update(dynamic context = null, params object[] args);
}