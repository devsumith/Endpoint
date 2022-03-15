using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Models
{


    public partial class SolutionModel
    {
        public List<ProjectModel> Projects { get; private set; } = new();
        public string Name { get; private set; }
        public string SrcDirectory => $"{SolutionDirectory}{Path.DirectorySeparatorChar}src";
        public string SolutionPath => $"{SolutionDirectory}{Path.DirectorySeparatorChar}{Name}.sln";
        public string Directory { get; private set; }
        public string SolutionDirectory => $"{Directory}{Path.DirectorySeparatorChar}{Name}";
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
