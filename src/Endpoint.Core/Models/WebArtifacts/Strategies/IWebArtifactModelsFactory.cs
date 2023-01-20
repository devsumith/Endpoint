namespace Endpoint.Core.Models.WebArtifacts.Strategies;

public interface IWebArtifactModelsFactory
{
    AngularProjectModel Create(string name, string prefix, string directory);
}
