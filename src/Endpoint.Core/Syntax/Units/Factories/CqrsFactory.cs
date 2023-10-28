// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Xml.Linq;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Units;
using MediatR;
using Microsoft.Extensions.Logging;
using Octokit;
using Octokit.Internal;

namespace Endpoint.Core.Syntax.Units.Factories;

public class CqrsFactory : ICqrsFactory
{
    private readonly ILogger<CqrsFactory> logger;
    private readonly INamingConventionConverter namingConventionConverter;
    private readonly IClassFactory classFactory;

    public CqrsFactory(ILogger<CqrsFactory> logger, INamingConventionConverter namingConventionConverter, IClassFactory classFactory)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        this.classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
    }

    public Task<CommandModel> CreateCommandAsync(string name, string properties)
    {
        logger.LogInformation("Creating Cqrs Command. {name}", name);

        throw new NotImplementedException();
    }

    public async Task<QueryModel> CreateQueryAsync(string routeType, string aggregateName, string properties)
    {
        logger.LogInformation("Creating Cqrs Query. {name}", aggregateName);

        return new ()
        {
            Name = routeType.ToRequestName(namingConventionConverter),

            // Response = await classFactory.CreateResponseAsync(routeType.ToRequestType(), aggregateName),

            // Request = await classFactory.CreateResponseAsync(routeType.ToRequestType(), aggregateName),
            RequestHandler = null,
        };
    }
}
