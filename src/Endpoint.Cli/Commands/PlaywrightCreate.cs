// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.WebArtifacts.Services;

namespace Endpoint.Cli.Commands;


[Verb("playwright-create")]
public class PlaywrightCreateRequest : IRequest {
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class PlaywrightCreateRequestHandler : IRequestHandler<PlaywrightCreateRequest>
{
    private readonly ILogger<PlaywrightCreateRequestHandler> _logger;
    private readonly IPlaywrightService _playwrightService;

    public PlaywrightCreateRequestHandler(
        ILogger<PlaywrightCreateRequestHandler> logger,
        IPlaywrightService playwrightService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _playwrightService = playwrightService ?? throw new ArgumentNullException(nameof(playwrightService));
    }

    public async Task Handle(PlaywrightCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(PlaywrightCreateRequestHandler));

        _playwrightService.Create(request.Directory);
    }
}
