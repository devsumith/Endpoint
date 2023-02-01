// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Endpoint.Core.Models.Syntax.Classes;

namespace Endpoint.Core.Models.Syntax;

public class NamespaceModel
{
    public List<string> Usings { get; set; }
    public string Name { get; set; }
    public List<ClassModel> Classes { get; set; }
}

