// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Entities.Aggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using Octokit.Internal;
using Octokit;
using System.Xml.Linq;
using Endpoint.Core.Syntax.Classes.Factories;

namespace Endpoint.Core.Syntax.Cqrs;

public class CqrsFactory: ICqrsFactory
{
    private readonly ILogger<CqrsFactory> _logger;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly IClassFactory _classFactory;

    public CqrsFactory(ILogger<CqrsFactory> logger, INamingConventionConverter namingConventionConverter, IClassFactory classFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
    }

    public Task<CommandModel> CreateCommandAsync(string name, string properties)
    {
        _logger.LogInformation("Creating Cqrs Command. {name}", name);

        throw new NotImplementedException();
    }

    public async Task<QueryModel> CreateQueryAsync(string routeType, string aggregateName, string properties)
    {
        _logger.LogInformation("Creating Cqrs Query. {name}", aggregateName); ;

        var aggregateNamePascalCasePlural = _namingConventionConverter.Convert(NamingConvention.PascalCase, aggregateName, pluralize: true);

        var model = new QueryModel();

        model.Name = routeType switch
        {
            "get" => $"Get{aggregateNamePascalCasePlural}",
            "getbyid" => $"Get{aggregateName}ById",
            "page" => $"Get{aggregateNamePascalCasePlural}Page",
            _ => throw new NotSupportedException()
        };

        model.Response = null; // new ResponseModel(entity, routeType, namingConventionConverter);

        model.Request = await _classFactory.CreateQueryAsync(model.Name, properties);

        model.RequestHandler = null;
/*        model.RequestHandler = routeType switch
        {
            RouteType.Get => new GetRequestHandlerModel(entity, Request, Response, routeType, microserviceName, namingConventionConverter),
            RouteType.GetById => new GetByIdRequestHandlerModel(entity, Request, Response, routeType, microserviceName, namingConventionConverter),
            RouteType.Page => new PageRequestHandlerModel(entity, Request, Response, routeType, microserviceName, namingConventionConverter),

            _ => throw new NotSupportedException()
        }*/;

        return model;
    }
}

