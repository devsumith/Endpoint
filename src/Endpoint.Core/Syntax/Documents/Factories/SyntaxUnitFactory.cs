// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Properties;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Documents.Factories;

public class SyntaxUnitFactory : ISyntaxUnitFactory
{
    private readonly ILogger<SyntaxUnitFactory> logger;
    private readonly IClassFactory classFactory;

    public SyntaxUnitFactory(ILogger<SyntaxUnitFactory> logger, IClassFactory classFactory)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
    }

    public async Task CreateAsync()
    {
        logger.LogInformation("Create");
    }

    public async Task<AggregateModel> CreateAsync(string name, List<PropertyModel> properties)
    {
        logger.LogInformation("Create Aggregate Model. {name}", name);

        var aggregate = new ClassModel(name);

        var serviceName = string.Empty;

        var model = new AggregateModel()
        {
            Queries = new List<QueryModel>(),
            Commands = new List<CommandModel>(),
            Aggregate = aggregate,
            AggregateDto = aggregate.CreateDto(),
            AggregateExtensions = await classFactory.DtoExtensionsCreateAsync(aggregate),
        };

        foreach (var routeType in new[]
        {
            RouteType.Page,
            RouteType.Get,
            RouteType.GetById,
            RouteType.Delete,
            RouteType.Create,
            RouteType.Update,
        })
        {
            switch (routeType)
            {
                case RouteType.Page:
                    break;

                case RouteType.Get:
                case RouteType.GetById:
                    model.Queries.Add(new QueryModel()); // (serviceName, null, aggregate, routeType));
                    break;

                case RouteType.Delete:
                case RouteType.Create:
                case RouteType.Update:
                    model.Commands.Add(new CommandModel()); // new CommandModel(serviceName, aggregate, null, routeType));
                    break;
            }
        }

        return model;
    }
}
