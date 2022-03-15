using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public partial class FileModel
    {
        public string Template { get; private set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string Directory { get; private set; }
        public string Extension { get; private set; }
        public string Path => $"{Directory}{System.IO.Path.DirectorySeparatorChar}{Name}.{Extension}";
        public Dictionary<string, object> Tokens { get; private set; } = new();

        public FileModel()
        {
                
        }
    }
}
