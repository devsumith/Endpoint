// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Entities.Aggregate;
using Endpoint.Core.Syntax.Methods.Factories;
using Endpoint.Core.Syntax.Properties;
using EnvDTE;
using Microsoft.Extensions.Logging;
using SimpleNLG;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace Endpoint.Core.Syntax.AggregateModels;

public class AggregateModelFactory: IAggregateModelFactory
{
    private readonly ILogger<AggregateModelFactory> _logger;
    private readonly IClassFactory _classFactory;

    public AggregateModelFactory(ILogger<AggregateModelFactory> logger, IClassFactory classFactory){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
    }

    public async Task<AggregateModel> CreateAsync(string name, List<PropertyModel> properties)
    {
        _logger.LogInformation("Create Aggregate Model. {name}", name);

        var aggregate = new ClassModel(name);

        var serviceName = "";

        var model = new AggregateModel()
        {
            Queries = new List<QueryModel>(),
            Commands = new List<CommandModel>(),
            Aggregate = aggregate,
            AggregateDto = aggregate.CreateDto(),
            AggregateExtensions = await _classFactory.DtoExtensionsCreateAsync(aggregate)
        };


        foreach (var routeType in new[]
        {
            RouteType.Page,
            RouteType.Get,
            RouteType.GetById,
            RouteType.Delete,
            RouteType.Create,
            RouteType.Update
        })
        {
            switch (routeType)
            {
                case RouteType.Page:
                    break;

                case RouteType.Get:
                case RouteType.GetById:
                    model.Queries.Add(new QueryModel(serviceName, null, aggregate, routeType));
                    break;

                case RouteType.Delete:
                case RouteType.Create:
                case RouteType.Update:
                    model.Commands.Add(new CommandModel(serviceName, aggregate, null, routeType));
                    break;
            }
        }

        return model;
    }

}

