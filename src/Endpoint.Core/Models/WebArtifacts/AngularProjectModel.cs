namespace Endpoint.Core.Models.WebArtifacts;

public class AngularProjectModel: WebModel
{
	
	public AngularProjectModel(string name, string directory, string prefix = "app", AngularWorkspaceModel workspace = null)
	{
		Workspace = workspace;
		Name = name;
		Directory = directory;
		Prefix = prefix;
	}

	public string Name { get; set; }
	public string Prefix { get; set; }
	public string Directory { get; set; }

	public AngularWorkspaceModel Workspace { get; init; }
}
