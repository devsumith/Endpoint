// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;

namespace Endpoint.Core.Models.Syntax.Classes;

public class RequestModel: ClassModel {
    public RequestModel(ClassModel entity, RouteType routeType, INamingConventionConverter namingConventionConverter)
        :base("")
    {
        Name= routeType switch
        {
            RouteType.Create => $"Create{entity.Name}Request",
            RouteType.Update => $"Update{entity.Name}Request",
            RouteType.Delete => $"Delete{entity.Name}Request",
            _ => throw new NotSupportedException()
        };
    }

}

