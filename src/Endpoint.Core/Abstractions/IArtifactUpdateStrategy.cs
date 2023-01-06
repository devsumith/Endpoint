namespace Endpoint.Core.Abstractions;

public interface IArtifactUpdateStrategy
{
    int Priority { get; }
    bool CanHandle(dynamic configuration = null, params object[] args);

    void Update(dynamic configuration = null, params object[] args);
}