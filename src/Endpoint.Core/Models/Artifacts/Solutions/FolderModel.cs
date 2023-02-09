// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Projects;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public class FolderModel
{
	public FolderModel(string name, string parentDirectory)
	{
		Name = name;
		Directory = $"{parentDirectory}{Path.DirectorySeparatorChar}{name}";
		SubFolders = new List<FolderModel>();
		Projects = new List<ProjectModel>();
		Files = new List<FileModel>();
	}

	public string Name { get; set; }
	public string Directory { get; set; }
	public List<ProjectModel> Projects { get; set; }
	public List<FolderModel> SubFolders { get; set; }
	public List<FileModel> Files { get; set; }
}

