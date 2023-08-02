// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Endpoint.Core.Syntax.Entities;

namespace Endpoint.Core.Options;

public class SolutionSettingsModel
{
    public SolutionSettingsModel()
    {

    }
    public string Path => $"{Directory}{System.IO.Path.DirectorySeparatorChar}CliSettings.{Namespace}.json";
    public string Namespace { get; set; }
    public List<EntityModel> Entities { get; set; } = new List<EntityModel>();
    public string Directory { get; set; }
    public List<string> Metadata { get; set; } = new List<string>();
}

