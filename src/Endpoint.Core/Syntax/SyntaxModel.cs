// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.Syntax;

public class SyntaxModel
{
    public SyntaxModel()
    {
        Usings = new List<UsingModel>();
    }

    public List<UsingModel> Usings { get; set; }
}
