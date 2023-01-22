using System.IO;
using System.Text;

namespace Endpoint.Core.Models.WebArtifacts;

public class AngularProjectModel
{
	
	public AngularProjectModel(string name, string projectType, string prefix, string rootDirectory)
	{
		Name = name;
		RootDirectory = rootDirectory;
		ProjectType = projectType;
		Prefix = prefix;

		if(name.StartsWith('@'))
		{
            var parts = name.Split('/');

            var stringBuilder = new StringBuilder();
			
			stringBuilder.Append($"{RootDirectory}{Path.DirectorySeparatorChar}projects{Path.DirectorySeparatorChar}");

            stringBuilder.Append($"{parts[0].Replace("@", string.Empty)}{Path.DirectorySeparatorChar}{parts[1]}");

			Directory = stringBuilder.ToString();
		}
		else
		{
            Directory = $"{RootDirectory}{Path.DirectorySeparatorChar}projects{Path.DirectorySeparatorChar}{name}";
        }
		
	}

    public string Root { get; set; }
    public string Name { get; set; }
	public string Prefix { get; set; }
	public string Directory { get; set; }
    public string RootDirectory { get; set; }
    public string ProjectType { get; set; } = "application";
}
