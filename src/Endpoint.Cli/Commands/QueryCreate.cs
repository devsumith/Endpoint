// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Syntax.Units.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("query-create")]
public class QueryCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('a', "aggregate")]
    public string Aggregate { get; set; }

    [Option('p', "properties")]
    public string Properties { get; set; }

    [Option('r', "route-type")]
    public string RouteType { get; set; } = "get";

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class QueryCreateRequestHandler : IRequestHandler<QueryCreateRequest>
{
    private readonly ILogger<QueryCreateRequestHandler> logger;
    private readonly IAggregateService aggregateService;

    public QueryCreateRequestHandler(
        ILogger<QueryCreateRequestHandler> logger,
        IAggregateService aggregateService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.aggregateService = aggregateService ?? throw new ArgumentNullException(nameof(aggregateService));
    }

    public async Task Handle(QueryCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating Query. {name}", request.Name);

        await aggregateService.QueryCreateAsync(request.RouteType, request.Name, request.Aggregate, request.Properties, request.Directory);
    }
}
