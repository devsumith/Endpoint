namespace Endpoint.Cli.Models
{
    public class Settings
    {
        public string Projects { get; set; }
        public string Path { get; set; }
        public string SolutionName { get; set; }
        public string RootNamespace { get; set; }
        public string ApiNamespace { get; set; }
        public string DomainNamespace { get; set; }
        public string ApplicationNamespace { get; set; }
        public string InfrastructureNamespace { get; set; }
        public string Namespace { get; set; }
        public string Domain { get; set; }
        public string Core { get; set; }
        public string Api { get; set; }
        public string EntityIdDataType { get; set; }
        public string SourceFolder { get; set; }
        public string TestFolder { get; set; }
        public string DbContext { get; set; }

        public static Settings Empty => new();
    }
}
