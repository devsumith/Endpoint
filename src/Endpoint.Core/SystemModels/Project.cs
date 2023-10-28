// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Projects.Enums;

namespace Endpoint.Core.SystemModels;

public class Project
{
    public Project(DotNetProjectType type, LibraryType libraryType, string name, string directory)
    {
        Type = type;
        Name = name;
        Directory = directory;
        LibraryType = libraryType;
    }

    public string Name { get; set; }

    public string Directory { get; set; }

    public DotNetProjectType Type { get; set; }

    public LibraryType LibraryType { get; set; }
}
