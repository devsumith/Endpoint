namespace Endpoint.Core.Models.WebArtifacts;

public class WebArtifactModelsFactory: IWebArtifactModelsFactory
{
    public AngularProjectModel Create(string name, string prefix, string directory)
        => new AngularProjectModel(name, prefix, directory);

}
