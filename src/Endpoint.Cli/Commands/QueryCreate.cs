// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.Syntax.Entities.Aggregate;

namespace Endpoint.Cli.Commands;


[Verb("query-create")]
public class QueryCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('a', "aggregate")]
    public string Aggregate { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class QueryCreateRequestHandler : IRequestHandler<QueryCreateRequest>
{
    private readonly ILogger<QueryCreateRequestHandler> _logger;
    private readonly IAggregateService _aggregateService;

    public QueryCreateRequestHandler(
        ILogger<QueryCreateRequestHandler> logger,
        IAggregateService aggregateService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _aggregateService = aggregateService ?? throw new ArgumentNullException(nameof(aggregateService));
    }

    public async Task Handle(QueryCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(QueryCreateRequestHandler));

        _aggregateService.QueryCreate(request.Name, request.Aggregate, request.Directory);

    }
}
