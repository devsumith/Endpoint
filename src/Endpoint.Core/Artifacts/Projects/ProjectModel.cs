// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Folders;
using Endpoint.Core.Artifacts.Projects.Enums;
using Endpoint.Core.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Endpoint.Core.Artifacts.Projects;

public class ProjectModel
{
    public string Name { get; init; }
    public string Directory { get; init; }
    public string Path => $"{Directory}{System.IO.Path.DirectorySeparatorChar}{Name}.csproj";
    public string Namespace => Name;
    public DotNetProjectType DotNetProjectType { get; set; }
    public List<FileModel> Files { get; init; } = new List<FileModel>();
    public List<PackageModel> Packages { get; init; } = new();
    public bool HasSecrets { get; init; }
    public bool IsNugetPackage { get; init; }
    public int Order { get; set; } = 0;
    public bool GenerateDocumentationFile { get; set; }
    public List<string> Metadata { get; set; } = new List<string>();
    public List<string> References { get; set; }
    public List<FolderModel> Folders { get; set; }

    public ProjectModel(string dotNetProjectType, string name, string parentDirectory, List<string> references = null)
        : this(dotNetProjectType switch
        {
            "web" => DotNetProjectType.Web,
            "webapi" => DotNetProjectType.WebApi,
            "classlib" => DotNetProjectType.ClassLib,
            "worker" => DotNetProjectType.Worker,
            "xunit" => DotNetProjectType.XUnit,
            _ => DotNetProjectType.Console
        }, name, parentDirectory, references)
    {
        Folders = new List<FolderModel>();
    }

    public string GetApplicationUrl(IFileSystem fileSystem)
    {
        var launchSettingsPath = System.IO.Path.Combine(Directory, "Properties", "launchSettings.json");

        var json = JsonSerializer.Deserialize<JsonNode>(fileSystem.ReadAllText(launchSettingsPath));

        var applicationUrl = $"{json["profiles"]["https"]["applicationUrl"]}".Split(";").First();

        return $"{applicationUrl}/";
    }
    public ProjectModel(DotNetProjectType dotNetProjectType, string name, string parentDirectory, List<string> references = null)
    {
        DotNetProjectType = dotNetProjectType;
        Name = name;
        Directory = $"{parentDirectory}{System.IO.Path.DirectorySeparatorChar}{name}";
        References = references ?? new List<string>();
        Folders = new List<FolderModel>();
    }

    public ProjectModel(string name, string parentDirectory)
    {
        DotNetProjectType = DotNetProjectType.ClassLib;
        Name = name;
        Directory = $"{parentDirectory}{System.IO.Path.DirectorySeparatorChar}{name}";
        References = new List<string>();
        Folders = new List<FolderModel>();
    }

    public ProjectModel()
    {
        References = new List<string>();
        Folders = new List<FolderModel>();
    }
}

