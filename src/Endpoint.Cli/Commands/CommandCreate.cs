// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Syntax.Units.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Endpoint.Cli.Commands;


[Verb("command-create")]
public class CommandCreateRequest : IRequest
{

    [Option('n')]
    public string Name { get; set; }

    [Option('a', "aggregateName")]
    public string Aggregate { get; set; }

    [Option('p', "properties")]
    public string Properties { get; set; }

    [Option('r', "route-type")]
    public string RouteType { get; set; }

    [Option('d')]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class CommandCreateRequestHandler : IRequestHandler<CommandCreateRequest>
{
    private readonly IAggregateService _aggregateService;
    private readonly ILogger<CommandCreateRequestHandler> _logger;

    public CommandCreateRequestHandler(IAggregateService aggregateService, ILogger<CommandCreateRequestHandler> logger)
    {
        _aggregateService = aggregateService ?? throw new ArgumentNullException(nameof(aggregateService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(CommandCreateRequest request, CancellationToken cancellationToken)
    {
        await _aggregateService.CommandCreate(request.RouteType, request.Name, request.Aggregate, request.Properties, request.Directory);
    }
}

