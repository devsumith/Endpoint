using Endpoint.Core.Models.WebArtifacts;

namespace Endpoint.Core.Abstractions;

public interface IWebGenerationStrategyFactory
{
    void CreateFor(WebModel model, dynamic configuration = null);
}
