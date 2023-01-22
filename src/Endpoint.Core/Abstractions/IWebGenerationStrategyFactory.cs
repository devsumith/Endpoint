using Endpoint.Core.Models.WebArtifacts;

namespace Endpoint.Core.Abstractions;

public interface IWebGenerationStrategyFactory
{
    void CreateFor(LitWorkspaceModel model, dynamic context = null);
}
