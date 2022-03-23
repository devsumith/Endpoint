namespace Endpoint.Core.Models
{
    public class FileModel
    {
        public string Name { get; init; }
        public string Directory { get; init; }
        public string Extension { get; init; }
        public string Path => $"{Directory}{System.IO.Path.DirectorySeparatorChar}{Name}.{Extension}";

        public FileModel()
        {

        }
    }
}
