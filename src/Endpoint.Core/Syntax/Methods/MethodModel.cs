// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Attributes;
using Endpoint.Core.Syntax.Expressions;
using Endpoint.Core.Syntax.Params;
using Endpoint.Core.Syntax.Types;
using System.Collections.Generic;

namespace Endpoint.Core.Syntax.Methods;

public class MethodModel: SyntaxModel
{
    public MethodModel()
    {
        Params = new();
        Attributes = new();
        ReturnType = new("void");
    }

    public TypeDeclarationModel ParentType { get; set; }
    public List<ParamModel> Params { get; set; }
    public List<AttributeModel> Attributes { get; set; }
    public AccessModifier AccessModifier { get; set; }
    public string Name { get; set; }
    public bool Override { get; set; }
    public TypeModel ReturnType { get; set; }
    public ExpressionModel Body { get; set; }
    public bool Interface { get; set; }
    public bool Async { get; set; }
    public bool Static { get; set; }
}

