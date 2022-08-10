namespace Endpoint.Core
{
    public static class CoreConstants
    {
        public static string RootNamespace = nameof(Endpoint);
        public static string SettingsFileName = "clisettings.json";

        public static class ApiFileTemplates
        {
            public static readonly string AppSettings = nameof(AppSettings);
            public static readonly string Dependencies = nameof(Dependencies);
        }
        public static class DotNetTemplateTypes
        {
            public static readonly string WebApi = "webapi";
            public static readonly string XUnit = "xunit";
            public static readonly string ClassLibrary = "classlib";
        }
        public static class Folders
        {
            public static string Extensions = nameof(Extensions);
            public static string Features = nameof(Features);
            public static string Models = nameof(Models);
            public static string Data = nameof(Data);
            public static string Controllers = nameof(Controllers);
            public static string Core = nameof(Core);
            public static string Behaviors = nameof(Behaviors);
            public static string Properties = nameof(Properties);
            public static string Interfaces = nameof(Interfaces);
            public static string Logs = nameof(Logs);
        }

        public static class EnvironmentVariables
        {
            public const string DefaultCommand = $"Endpoint:{nameof(DefaultCommand)}";
        }

        public static class SolutionTemplates
        {
            public const string CleanArchitectureByJasonTalyor = nameof(CleanArchitectureByJasonTalyor);
            public const string CleanArchitectureBySteveSmith = nameof(CleanArchitectureBySteveSmith);
            public const string Minimal = nameof(Minimal);
            public const string Library = nameof(Library);
        }

        public static class TargetFramework
        {
            public const string NetCore6 = nameof(NetCore6);
            public const string NetCore7 = nameof(NetCore7);
            public const string Framework48 = nameof(Framework48);
        }

        public static class SolutionTypes
        {
            public const string Workspace = nameof(Workspace);
        }

        public static class ProjectType
        {
            public const string Domain = nameof(Domain);
            public const string Core = nameof(Core);
            public const string SharedKernel = nameof(SharedKernel);
            public const string Infrastructure = nameof(Infrastructure);
            public const string Application = nameof(Application);
            public const string Api = nameof(Api);
            public const string Cli = nameof(Cli);            
        }
    }
}
