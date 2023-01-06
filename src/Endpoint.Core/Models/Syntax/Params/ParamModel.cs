﻿using Endpoint.Core.Models.Syntax.Attributes;

namespace Endpoint.Core.Models.Syntax.Params;

public class ParamModel
{
    public string Name { get; set; }
    public TypeModel Type { get; set; }
    public AttributeModel Attribute { get; set; }
    public string DefaultValue { get; set; }
    public bool ExtensionMethodParam { get; set; }
}