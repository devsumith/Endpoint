// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Properties;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.TypeScript;

public class TypeScriptTypeModel {

    public TypeScriptTypeModel(string name)
    {
        Name = name;
        Properties = new List<PropertyModel>();
    }

    public string Name { get; set; }
    public List<PropertyModel> Properties { get; set; }
}

