// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts.Solutions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("event-driven-microservices-create")]
public class EventDrivenMicroservicesCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('s', "services")]
    public string Services { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class EventDrivenMicroservicesCreateRequestHandler : IRequestHandler<EventDrivenMicroservicesCreateRequest>
{
    private readonly ILogger<EventDrivenMicroservicesCreateRequestHandler> _logger;
    private readonly ISolutionService _solutionService;
    public EventDrivenMicroservicesCreateRequestHandler(
        ILogger<EventDrivenMicroservicesCreateRequestHandler> logger,
        ISolutionService solutionService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
    }

    public async Task Handle(EventDrivenMicroservicesCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(EventDrivenMicroservicesCreateRequestHandler));

        await _solutionService.EventDrivenMicroservicesCreate(request.Name, request.Services, request.Directory);


    }
}
