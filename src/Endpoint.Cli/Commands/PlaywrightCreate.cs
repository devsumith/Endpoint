// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("playwright-create")]
public class PlaywrightCreateRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class PlaywrightCreateRequestHandler : IRequestHandler<PlaywrightCreateRequest>
{
    private readonly ILogger<PlaywrightCreateRequestHandler> logger;
    private readonly IPlaywrightService playwrightService;

    public PlaywrightCreateRequestHandler(
        ILogger<PlaywrightCreateRequestHandler> logger,
        IPlaywrightService playwrightService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.playwrightService = playwrightService ?? throw new ArgumentNullException(nameof(playwrightService));
    }

    public async Task Handle(PlaywrightCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(PlaywrightCreateRequestHandler));

        playwrightService.Create(request.Directory);
    }
}
