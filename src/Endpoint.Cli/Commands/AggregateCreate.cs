// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.Syntax.Entities.Aggregate;
using Microsoft.Extensions.Configuration;

namespace Endpoint.Cli.Commands;


[Verb("aggregate-create")]
public class AggregateCreateRequest : IRequest<Unit> {
    [Option('n',"name")]
    public string Name { get; set; }

    [Option('p', "properties")]
    public string Properties { get; set; }


    [Option('m', "microservice-name")]
    public string MicroserviceName { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AggregateCreateRequestHandler : IRequestHandler<AggregateCreateRequest, Unit>
{
    private readonly ILogger<AggregateCreateRequestHandler> _logger;
    private readonly IAggregateService _aggregateService;

    public AggregateCreateRequestHandler(
        ILogger<AggregateCreateRequestHandler> logger,
        IAggregateService aggregateService
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _aggregateService = aggregateService ?? throw new ArgumentNullException(nameof(aggregateService));
    }

    public async Task<Unit> Handle(AggregateCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AggregateCreateRequestHandler));

        await _aggregateService.Add(request.Name, request.Properties, request.Directory, request.MicroserviceName);

        return new();
    }
}
