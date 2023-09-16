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

[Verb("test-create")]
public class TestCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class TestCreateRequestHandler : IRequestHandler<TestCreateRequest>
{
    private readonly ILogger<TestCreateRequestHandler> logger;
    private readonly IPlaywrightService playwrightService;

    public TestCreateRequestHandler(
        ILogger<TestCreateRequestHandler> logger,
        PlaywrightService playwrightService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.playwrightService = playwrightService ?? throw new ArgumentNullException(nameof(playwrightService));
    }

    public async Task Handle(TestCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(TestCreateRequestHandler));
    }
}
