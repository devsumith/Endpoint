// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.WebArtifacts.Services;

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
    private readonly ILogger<TestCreateRequestHandler> _logger;
    private readonly IPlaywrightService _playwrightService;

    public TestCreateRequestHandler(
        ILogger<TestCreateRequestHandler> logger,
        PlaywrightService playwrightService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _playwrightService = playwrightService ?? throw new ArgumentNullException(nameof(playwrightService));
    }

    public async Task Handle(TestCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(TestCreateRequestHandler));
    }
}
