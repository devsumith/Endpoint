namespace Endpoint.Core.Models
{
    public class PackageModel
    {
        public string Name { get; init; }
        public string Version { get; init; }
        public bool IsPreRelease { get; init; }

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
