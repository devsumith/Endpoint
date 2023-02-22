// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Types;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.TypeScript;

public class ImportModel
{

    public ImportModel()
    {
        Types = new List<TypeModel>();
    }

    public ImportModel(string type, string module)
    {
        Module = module;
        Types = new List<TypeModel>
        {
            new TypeModel(type)
        };
    }
    public List<TypeModel> Types { get; set; }
    public string Module { get; set; }
}

