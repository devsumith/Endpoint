namespace Endpoint.Cli.Models
{
    public class CliSettings
    {
        public string Projects { get; set; }
        public string Path { get; set; }
        public string DatabaseSettingsKey { get; set; }
        public string SolutionName { get; set; }
        public string RootNamespace { get; set; }
        public string Namespace { get; set; }
        public string Domain { get; set; }
        public string Core { get; set; }
        public string Api { get; set; }
        public string EntityIdDataType { get; set; }
        public string SourceFolder { get; set; }
        public string TestFolder { get; set; }
        public string SolutionPath => @$"C:\projects\{RootNamespace}";

    }
}
