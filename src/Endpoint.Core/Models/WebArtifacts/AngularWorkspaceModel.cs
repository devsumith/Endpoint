using System.IO;

namespace Endpoint.Core.Models.WebArtifacts;

public class AngularWorkspaceModel: WebModel
{
	public AngularWorkspaceModel(string name, string rootDirectory)
	{
		Name = name;
		RootDirectory = rootDirectory;
		Directory = $"{RootDirectory}{Path.DirectorySeparatorChar}{name}";
	}

    public string Name { get; set; }
    public string RootDirectory { get; set; }
    public string Directory { get; set; }
}