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

[Verb("ng-component-create")]
public class AngularComponentCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularComponentCreateRequestHandler : IRequestHandler<AngularComponentCreateRequest>
{
    private readonly ILogger<AngularComponentCreateRequestHandler> logger;
    private readonly IAngularService angularService;

    public AngularComponentCreateRequestHandler(
        ILogger<AngularComponentCreateRequestHandler> logger,
        IAngularService angularService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(AngularComponentCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating Angular Component.", nameof(AngularComponentCreateRequestHandler));

        await angularService.ComponentCreate(request.Name, request.Directory);
    }
}
