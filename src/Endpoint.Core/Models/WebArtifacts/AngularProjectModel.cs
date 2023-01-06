namespace Endpoint.Core.Models.WebArtifacts;

public class AngularProjectModel: WebModel
{
	
	public AngularProjectModel(string name, string prefix = "app", AngularWorkspaceModel workspace = null)
	{
		Workspace = workspace;
		Name = name;
		Prefix = prefix;
	}

	public string Name { get; set; }
	public string Prefix { get; set; }

	public AngularWorkspaceModel Workspace { get; init; }
}
