using Endpoint.Core.Models.WebArtifacts;

namespace Endpoint.Core.Abstractions;

public interface IWebGenerationStrategy
{
    bool CanHandle(LitWorkspaceModel model, dynamic context = null);
    void Create(LitWorkspaceModel model, dynamic context = null);
    int Priority { get; }
}
