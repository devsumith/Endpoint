using Endpoint.Core.Models.WebArtifacts;

namespace Endpoint.Core.Abstractions;

public interface IWebGenerationStrategy
{
    bool CanHandle(WebModel model, dynamic context = null);
    void Create(WebModel model, dynamic context = null);
    int Priority { get; }
}
