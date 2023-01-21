namespace Endpoint.Core.Models.WebArtifacts.Factories;

public class WebArtifactModelsFactory : IWebArtifactModelsFactory
{
    public AngularProjectModel Create(string name, string prefix, string directory)
        => new AngularProjectModel(name,null, prefix, directory);

}
