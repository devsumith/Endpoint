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


[Verb("ng-service-create")]
public class AngularServiceCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularServiceCreateRequestHandler : IRequestHandler<AngularServiceCreateRequest>
{
    private readonly ILogger<AngularServiceCreateRequestHandler> _logger;
    private readonly IAngularService _angularService;
    public AngularServiceCreateRequestHandler(
        ILogger<AngularServiceCreateRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(AngularServiceCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AngularServiceCreateRequestHandler));

        await _angularService.ServiceCreate(request.Name, request.Directory);


    }
}
