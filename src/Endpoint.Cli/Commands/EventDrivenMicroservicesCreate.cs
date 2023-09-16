// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Solutions.Services;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<EventDrivenMicroservicesCreateRequestHandler> logger;
    private readonly ISolutionService solutionService;

    public EventDrivenMicroservicesCreateRequestHandler(
        ILogger<EventDrivenMicroservicesCreateRequestHandler> logger,
        ISolutionService solutionService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
    }

    public async Task Handle(EventDrivenMicroservicesCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(EventDrivenMicroservicesCreateRequestHandler));

        await solutionService.EventDrivenMicroservicesCreate(request.Name, request.Services, request.Directory);
    }
}
