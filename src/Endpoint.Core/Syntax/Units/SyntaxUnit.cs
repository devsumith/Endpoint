// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.Syntax.Units;

public class SyntaxUnitModel : SyntaxModel
{

    public SyntaxUnitModel()
    {

    }

    public SyntaxUnitModel(string name)
    {
        Name = name;
        Code = new List<SyntaxModel>();
    }

    public string Name { get; set; }
    public List<SyntaxModel> Code { get; set; }
}
