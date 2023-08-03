// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Artifacts.Services;

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
    private readonly ILogger<AngularListComponentCreateRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public AngularListComponentCreateRequestHandler(
        ILogger<AngularListComponentCreateRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(AngularListComponentCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AngularListComponentCreateRequestHandler));

        await _angularService.ListComponentCreate(request.Name, request.Directory);


    }
}
