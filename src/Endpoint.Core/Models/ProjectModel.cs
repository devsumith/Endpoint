using Endpoint.Core.Models.Files;
using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public class ProjectModel
    {
        public string Name { get; private set; }
        public string Directory { get; private set; }
        public string Path => $"{Directory}{System.IO.Path.DirectorySeparatorChar}{Name}.csproj";
        public string Namespace => Name;
        public DotNetProjectType DotNetProjectType { get; set; }
        public List<FileModel> Files { get; private set; } = new List<FileModel>();
        public List<PackageModel> Packages { get; private set; } = new();
        public bool HasSecrets { get; init; }
        public bool IsNugetPackage { get; init; }
        public int Order { get; init; } = 0;
        public bool GenerateDocumentationFile { get; set; }
        public List<string> Metadata { get; set; } = new List<string>();

        public ProjectModel(DotNetProjectType dotNetProjectType, string name, string parentDirectory)
        {
            DotNetProjectType = dotNetProjectType;

            Name = name;

            Directory = $"{parentDirectory}{System.IO.Path.DirectorySeparatorChar}{name}";
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
}
