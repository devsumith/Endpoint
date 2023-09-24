// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Core.Syntax.Documents;

public class CommandModel : QueryModel
{
    public CommandModel()
    {
    }

    public ClassModel RequestValidator { get; set; }

    /*    public CommandModel(string microserviceName, ClassModel entity, INamingConventionConverter namingConventionConverter, RouteType routeType = default, string name = null)
        {
            Name = name ?? routeType switch
            {
                RouteType.Create => $"Create{entity.Name}",
                RouteType.Update => $"Update{entity.Name}",
                RouteType.Delete => $"Delete{entity.Name}",
                _ => throw new NotSupportedException()
            };

            Response = new ResponseModel(entity, routeType, namingConventionConverter);

            Request = new RequestModel(Response, entity, routeType, namingConventionConverter);

            RequestHandler = routeType switch
            {
                RouteType.Delete => new DeleteRequestHandlerModel(entity, Request, Response, routeType, microserviceName, namingConventionConverter),

                RouteType.Update => new UpdateRequestHandlerModel(entity, Request, Response, routeType, microserviceName, namingConventionConverter),

                RouteType.Create => new CreateRequestHandlerModel(entity, Request, Response, routeType, microserviceName, namingConventionConverter),

                _ => throw new NotSupportedException()
            };

            RequestValidator = new RequestValidatorModel(Request);
        }
    */
}
