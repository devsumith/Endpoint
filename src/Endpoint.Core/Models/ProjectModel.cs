using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public partial class ProjectModel
    {
        public string Name { get; private set; }
        public string Directory { get; private set; }
        public string Path => $"{Directory}{System.IO.Path.DirectorySeparatorChar}{Name}.csproj";
        public string Namespace => Name;
        public string Type { get; set; }
        public List<ProjectModel> References { get; set; } = new List<ProjectModel>();
        public List<FileModel> Files { get; private set; } = new List<FileModel>();
        public List<PackageModel> Packages { get; private set; } = new();
        public bool HasSecrets { get; init; }
        public bool IsNugetPackage { get; init; }
        public int Order { get; init; } = 0;

        public ProjectModel(string type, string name, string parentDirectory, List<ProjectModel> references)
            :this(type, name, parentDirectory)
        {
            References = references;
        }

        public ProjectModel(string type, string name, string parentDirectory)
        {
            Type = type;

            Name = name;

            Directory = $"{parentDirectory}{System.IO.Path.DirectorySeparatorChar}{name}";
        }

        public ProjectModel()
        {

        }
    }
}
