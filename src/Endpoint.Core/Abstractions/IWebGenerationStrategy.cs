using Endpoint.Core.Models.WebArtifacts;

namespace Endpoint.Core.Abstractions;

public interface IWebGenerationStrategy
{
    bool CanHandle(WebModel model, dynamic configuration = null);
    void Create(WebModel model, dynamic configuration = null);
    int Priority { get; }
}
