// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;
using System.Collections.Generic;

namespace Endpoint.Core.Syntax.Entities.Aggregate;

public class QueryModel
{
    public QueryModel()
    {
        UsingDirectives = new List<UsingDirectiveModel>();
    }
    public string Name { get; set; }
    public ClassModel Request { get; set; }
    public ClassModel Response { get; set; }
    public ClassModel RequestHandler { get; set; }
    public List<UsingDirectiveModel> UsingDirectives { get; set; }
}

