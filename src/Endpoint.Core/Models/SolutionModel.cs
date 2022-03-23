using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Models
{
    public class SolutionModel
    {
        public List<ProjectModel> Projects { get; private set; } = new();
        public string Name { get; init; }
        public string SrcDirectoryName { get; private set; } = "src";
        public string TestDirectoryName { get; private set; } = "test";
        public string SrcDirectory => $"{SolutionDirectory}{Path.DirectorySeparatorChar}{SrcDirectoryName}";
        public string TestDirectory => $"{SolutionDirectory}{Path.DirectorySeparatorChar}{TestDirectory}";
        public string SolutionPath => $"{SolutionDirectory}{Path.DirectorySeparatorChar}{Name}.sln";
        public string Directory { get; init; }
        public string SolutionDirectory => $"{Directory}{Path.DirectorySeparatorChar}{Name}";
        public List<DependsOnModel> DependOns { get; set; } = new();
        public SolutionModel(string name, string directory)
        {
            Name = name;
            Directory = directory;
        }

        public SolutionModel()
        {

        }
    }
}
