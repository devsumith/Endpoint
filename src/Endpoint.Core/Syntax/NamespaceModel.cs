// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Core.Syntax;

public class NamespaceModel
{
    public List<string> Usings { get; set; }
    public string Name { get; set; }
    public List<ClassModel> Classes { get; set; }
}

