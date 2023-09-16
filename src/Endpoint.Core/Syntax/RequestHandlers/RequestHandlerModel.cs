// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Entities;

namespace Endpoint.Core.Syntax.RequestHandlers;

public class RequestHandlerModel : ClassModel
{
    public RequestHandlerModel(string name)
        : base(name)
    {
    }

    public RouteType RouteType { get; set; }

    public EntityModel Entity { get; set; }
}
