using System.Collections.Generic;
using System.IO;
using System.Linq;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Projects;
using Endpoint.Core.Models.Syntax.Classes;

namespace Endpoint.Core.Models.Artifacts.Solutions
{
    public class SolutionModel
    {
        public SolutionModel()
        {
            DependOns = new List<DependsOnModel>();
            Projects = new List<ProjectModel>();
            Files = new List<FileModel>();
            Folders = new List<FolderModel>();
        }

        public string Name { get; init; }
        public string Directory { get; init; }

        public List<FolderModel> Folders { get; set; }
        public List<DependsOnModel> DependOns { get; set; }
        public List<ProjectModel> Projects { get; private set; }
        public List<FileModel> Files { get; set; }
        public SolutionModel(string name, string directory)
        {
            Name = name;
            Directory = directory;
            SolutionDirectory = $"{Directory}{Path.DirectorySeparatorChar}{Name}";
            Projects = new List<ProjectModel>();
            DependOns = new List<DependsOnModel>();
            Files = new List<FileModel>();
            Folders = new List<FolderModel>();
        }

        public SolutionModel(string name, string directory, string solutionDirectory)
        {
            Name = name;
            Directory = directory;
            SolutionDirectory = solutionDirectory;
        }

        public string SrcDirectoryName { get; private set; } = "src";
        public string TestDirectoryName { get; private set; } = "tests";
        public string SrcDirectory => $"{SolutionDirectory}{Path.DirectorySeparatorChar}{SrcDirectoryName}";
        public string TestDirectory => $"{SolutionDirectory}{Path.DirectorySeparatorChar}{TestDirectoryName}";
        public string SolutionPath => $"{SolutionDirectory}{Path.DirectorySeparatorChar}{Name}.sln";

        public string SolutionDirectory { get; set; }
        public string SolultionFileName => $"{Name}.sln";

        public ClassModel GetClass(string name, string serviceName)
        {
            foreach(var folder in  Folders)
            {
                foreach(var project in string.IsNullOrEmpty(serviceName) ? folder.Projects: folder.Projects.Where(x => x.Name == serviceName))
                {
                    foreach(var file in project.Files)
                    {
                        if(file is ObjectFileModel<ClassModel> classFileModel)
                        {
                            if (classFileModel.Object.Name.Split('.').Last() == name)                                
                                return classFileModel.Object;
                        }
                    }
                }
            }

            return null;
        }

    }
}
