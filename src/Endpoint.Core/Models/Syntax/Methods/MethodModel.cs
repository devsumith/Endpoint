// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax.Attributes;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Types;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.Methods;

public class MethodModel
{
    public MethodModel()
    {
        Params = new List<ParamModel>();
        Attributes = new List<AttributeModel>();
        ReturnType = new TypeModel("void");
    }

    public TypeDeclarationModel ParentType { get; set; }
    public List<ParamModel> Params { get; set; }
    public List<AttributeModel> Attributes { get; set; }
    public AccessModifier AccessModifier { get; set; }
    public string Name { get; set; }
    public bool Override { get; set; }
    public TypeModel ReturnType { get; set; }
    public string Body { get; set; }
    public bool Interface { get; set; }
    public bool Async { get; set; }
    public bool Static { get; set; }
}

