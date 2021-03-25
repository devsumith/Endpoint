using static System.IO.Path;

namespace Endpoint.Application.Models
{
    public class Settings
    {
        public string Projects { get; set; }
        public string Path { get; set; }
        public string SolutionName { get; set; }
        public string RootNamespace { get; set; }
        public string DomainNamespace { get; set; }
        public string ApplicationNamespace { get; set; }
        public string InfrastructureNamespace { get; set; }
        public string ApiNamespace { get; set; }
        public string DomainDirectory { get; set; }
        public string ApplicationDirectory { get; set; }
        public string InfrastructureDirectory { get; set; }
        public string ApiDirectory { get; set; }
        public string BuildingBlocksCoreNamespace { get; set; } = "BuildingBlocks.Core";
        public string BuildingBlocksEventStoreNamespace { get; set; } = "BuildingBlocks.EventStore";
        public string Store { get; set; }
        public string Namespace { get; set; }
        public string Domain { get; set; }
        public string Core { get; set; }
        public string Api { get; set; }
        public string EntityIdDataType { get; set; }
        public string SourceFolder { get; set; }
        public string TestFolder { get; set; }
        public string DbContext { get; set; }
        public bool EventSourcing { get; set; }
        public static Settings Empty => new();
    }
}
