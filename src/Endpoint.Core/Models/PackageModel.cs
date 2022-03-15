namespace Endpoint.Core.Models
{
    public class PackageModel
    {
        public string Name { get; set; }
        public string Version { get; set; }

        public PackageModel(string name, string verison)
        {
            Name = name;
            Version = verison;
        }

        public PackageModel()
        {

        }
    }
}
