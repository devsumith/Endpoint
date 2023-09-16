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

[Verb("angular-list-component-create")]
public class AngularListComponentCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularListComponentCreateRequestHandler : IRequestHandler<AngularListComponentCreateRequest>
{
    private readonly ILogger<AngularListComponentCreateRequestHandler> logger;
    private readonly IAngularService angularService;

    public AngularListComponentCreateRequestHandler(
        ILogger<AngularListComponentCreateRequestHandler> logger,
        IAngularService angularService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(AngularListComponentCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(AngularListComponentCreateRequestHandler));

        await angularService.ListComponentCreate(request.Name, request.Directory);
    }
}
