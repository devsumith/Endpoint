// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("scss-component-create")]
public class ScssComponentCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ScssComponentCreateRequestHandler : IRequestHandler<ScssComponentCreateRequest>
{
    private readonly ILogger<ScssComponentCreateRequestHandler> logger;
    private readonly IAngularService angularService;

    public ScssComponentCreateRequestHandler(
        ILogger<ScssComponentCreateRequestHandler> logger,
        IAngularService angularService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(ScssComponentCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(ScssComponentCreateRequestHandler));

        await angularService.ScssComponentCreate(request.Name, request.Directory);
    }
}
