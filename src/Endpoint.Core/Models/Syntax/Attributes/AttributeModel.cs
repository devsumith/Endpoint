// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Endpoint.Core.Models.Syntax.Params;

namespace Endpoint.Core.Models.Syntax.Attributes;

public class AttributeModel
{
    public AttributeType Type { get; init; }
    public string Name { get; init; }
    public Dictionary<string, string> Properties { get; init; }
    public List<ParamModel> Params { get; set; }
    public string Template { get; set; }
    public int Order { get; init; } = 0;

    public AttributeModel()
    {
        Properties = new Dictionary<string, string>();
        Params = new List<ParamModel>();
    }

    public AttributeModel(AttributeType type, string name, Dictionary<string, string> properties)
    {
        Type = type;
        Name = name;
        Properties = properties;
    }
}

