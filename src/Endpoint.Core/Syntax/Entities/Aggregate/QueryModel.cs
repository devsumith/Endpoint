// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Core.Syntax.Entities.Aggregate;

public class QueryModel : CqrsBase
{
    public QueryModel(string microserviceName, INamingConventionConverter namingConventionConverter, ClassModel entity, RouteType routeType = RouteType.Get, string name = null)
    {
        var entityNamePascalCasePlural = namingConventionConverter.Convert(NamingConvention.PascalCase, entity.Name, pluralize: true);

        Name = name ?? routeType switch
        {
            RouteType.Get => $"Get{entityNamePascalCasePlural}",
            RouteType.GetById => $"Get{entity.Name}ById",
            RouteType.Page => $"Get{entityNamePascalCasePlural}Page",
            _ => throw new NotSupportedException()
        };


        Response = new ResponseModel(entity, routeType, namingConventionConverter);

        Request = new RequestModel(Response, entity, routeType, namingConventionConverter);

        RequestHandler = routeType switch
        {
            RouteType.Get => new GetRequestHandlerModel(entity, Request, Response, routeType, microserviceName, namingConventionConverter),

            RouteType.GetById => new GetByIdRequestHandlerModel(entity, Request, Response, routeType, microserviceName, namingConventionConverter),

            RouteType.Page => new PageRequestHandlerModel(entity, Request, Response, routeType, microserviceName, namingConventionConverter),

            _ => throw new NotSupportedException()
        };

    }

}

