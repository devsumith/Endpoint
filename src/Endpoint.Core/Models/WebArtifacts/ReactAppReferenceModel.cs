namespace Endpoint.Core.Models.WebArtifacts;

public class ReactAppReferenceModel
{
    public ReactAppReferenceModel(string name, string referenceDirectory)
    {
        Name = name;
		ReferenceDirectory = referenceDirectory;
    }

    public string Name { get; set; }
    public string ReferenceDirectory { get; set; }
}
