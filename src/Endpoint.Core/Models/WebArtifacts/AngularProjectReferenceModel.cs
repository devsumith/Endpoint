namespace Endpoint.Core.Models.WebArtifacts;

public class AngularProjectReferenceModel
{
    public AngularProjectReferenceModel(string name, string referencedDirectory, string projectType = "application")
    {
        Name = name;
        ReferencedDirectory = referencedDirectory;
        ProjectType = projectType;
    }

    public string Name { get; set; }
    public string ReferencedDirectory { get; set; }
    public string ProjectType { get; set; }
}
