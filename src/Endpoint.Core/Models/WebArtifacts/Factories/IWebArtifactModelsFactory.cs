namespace Endpoint.Core.Models.WebArtifacts.Factories;

public interface IWebArtifactModelsFactory
{
    AngularProjectModel Create(string name, string prefix, string directory);
}
