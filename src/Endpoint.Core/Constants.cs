namespace Endpoint.SharedKernal
{
    public static class Constants
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
    }
}
