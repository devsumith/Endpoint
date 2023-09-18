// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Expressions;
using Endpoint.Core.Syntax.Params;

namespace Endpoint.Core.Syntax.Constructors;

public class ConstructorModel : SyntaxModel
{
    public ConstructorModel(ClassModel @class, string name)
    {
        Parent = @class;
        Name = name;
        Params = new List<ParamModel>();
        BaseParams = new List<string>();
    }

    public string Name { get; set; }

    public AccessModifier AccessModifier { get; set; }

    public ExpressionModel Body { get; set; }

    public List<string> BaseParams { get; set; }

    public List<ParamModel> Params { get; set; }

    public override IEnumerable<SyntaxModel> GetChildren()
    {
        foreach (var param in Params)
        {
            yield return param;
        }

        yield return Body;
    }
}
