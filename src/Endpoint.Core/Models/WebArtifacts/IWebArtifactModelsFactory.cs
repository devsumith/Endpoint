namespace Endpoint.Core.Models.WebArtifacts;

public interface IWebArtifactModelsFactory
{
    AngularProjectModel Create(string name, string prefix, string directory);
}
