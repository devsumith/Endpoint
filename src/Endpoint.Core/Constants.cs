// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;

namespace Endpoint.Core;

public static class Constants
{
    public static readonly string FileNotFound = nameof(FileNotFound);

    public static string RootNamespace = nameof(Endpoint);
    public static string SettingsFileName = "clisettings.json";
    public static string[] ExcludePatterns = new[]
    {
        $"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}",
        $"{Path.DirectorySeparatorChar}nupkg{Path.DirectorySeparatorChar}",
        $"{Path.DirectorySeparatorChar}Properties{Path.DirectorySeparatorChar}",
        $"{Path.DirectorySeparatorChar}node_modules{Path.DirectorySeparatorChar}",
    };

    public static class FileExtensions
    {
        public static string CSharpFile = ".cs";
        public static string TypeScriptFile = ".ts";
        public static string CSharpProject = ".csproj";
        public static string HtmlFile = ".html";
    }

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
