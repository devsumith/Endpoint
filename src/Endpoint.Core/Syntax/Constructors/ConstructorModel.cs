// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Params;

namespace Endpoint.Core.Syntax.Constructors;

public class ConstructorModel
{
    public ConstructorModel(ClassModel @class, string name)
    {
        Class = @class;
        Name = name;
        Params = new List<ParamModel>();
        BaseParams = new List<string>();
    }

    public ClassModel Class { get; set; }

    public string Name { get; set; }

    public AccessModifier AccessModifier { get; set; }

    public string Body { get; set; }

    public List<string> BaseParams { get; set; }

    public List<ParamModel> Params { get; set; }
}
