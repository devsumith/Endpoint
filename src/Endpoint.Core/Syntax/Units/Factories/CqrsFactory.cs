// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using MediatR;
using Microsoft.Extensions.Logging;
using Octokit.Internal;
using Octokit;
using System.Xml.Linq;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Units;

namespace Endpoint.Core.Syntax.Units.Factories;

public class CqrsFactory : ICqrsFactory
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

        return new()
        {
            Name = routeType.ToRequestName(_namingConventionConverter),

            Response = await _classFactory.CreateResponseAsync(routeType.ToRequestType(), aggregateName),

            Request = await _classFactory.CreateResponseAsync(routeType.ToRequestType(), aggregateName),

            RequestHandler = null
        }; ;
    }
}

