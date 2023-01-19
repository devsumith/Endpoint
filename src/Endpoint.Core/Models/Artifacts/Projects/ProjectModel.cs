using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Projects.Enums;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Artifacts.Projects;

public class ProjectModel
{
    public string Name { get; init; }
    public string Directory { get; init; }
    public string Path => $"{Directory}{System.IO.Path.DirectorySeparatorChar}{Name}.csproj";
    public string Namespace => Name;
    public DotNetProjectType DotNetProjectType { get; set; }
    public List<FileModel> Files { get; init; } = new List<FileModel>();
    public List<PackageModel> Packages { get; init; } = new();
    public bool HasSecrets { get; init; }
    public bool IsNugetPackage { get; init; }
    public int Order { get; init; } = 0;
    public bool GenerateDocumentationFile { get; set; }
    public List<string> Metadata { get; set; } = new List<string>();
    public List<string> References { get; set; }

    public ProjectModel(string dotNetProjectType, string name, string parentDirectory, List<string> references = null)
        :this(dotNetProjectType switch
        {
            "web" => DotNetProjectType.Web,
            "webapi" => DotNetProjectType.WebApi,
            "classlib" => DotNetProjectType.ClassLib,
            "worker" => DotNetProjectType.Worker,
            "xunit" => DotNetProjectType.XUnit,
            _ => DotNetProjectType.Console
        },name,parentDirectory,references)
    {
    }

    public ProjectModel(DotNetProjectType dotNetProjectType, string name, string parentDirectory, List<string> references = null)
    {
        DotNetProjectType = dotNetProjectType;
        Name = name;
        Directory = $"{parentDirectory}{System.IO.Path.DirectorySeparatorChar}{name}";
        References = references;
    }

    public ProjectModel(string name, string parentDirectory)
    {
        DotNetProjectType = DotNetProjectType.ClassLib;

        Name = name;

        Directory = $"{parentDirectory}{System.IO.Path.DirectorySeparatorChar}{name}";
    }

    public ProjectModel()
    {

    }
}
