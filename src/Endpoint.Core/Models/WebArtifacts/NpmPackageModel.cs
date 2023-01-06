namespace Endpoint.Core.Models.WebArtifacts;

public class NpmPackageModel
{
    public NpmPackageModel(string name, string version)
    {
        Name = name;
        Version = version;
    }

    public string Name { get; set; }
    public string Version { get; set; }
}
