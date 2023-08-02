// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.Syntax.TypeScript;

public class FunctionModel
{

    public FunctionModel()
    {
        Imports = new List<ImportModel>();
    }
    public string Name { get; set; }
    public string Body { get; set; }
    public List<ImportModel> Imports { get; set; }
}

