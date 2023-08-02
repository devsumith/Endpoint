// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;
using System.Collections.Generic;

namespace Endpoint.Core.Syntax.Entities.Aggregate;

public class CqrsBase
{
    public CqrsBase()
    {
        UsingDirectives = new List<UsingDirectiveModel>();
    }
    public string Name { get; set; }
    public RequestModel Request { get; set; }
    public ResponseModel Response { get; set; }
    public ClassModel RequestHandler { get; set; }
    public List<UsingDirectiveModel> UsingDirectives { get; set; }
}