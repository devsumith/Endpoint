using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Models
{
    public class WorkspaceModel
    {
        public string Name { get; set; }
        public string Directory => $"{ParentDirectory}{Path.DirectorySeparatorChar}{Name}";
        public string ParentDirectory { get; init; }
        public List<SolutionModel> Solutions { get; private set; } = new();
    }
}
