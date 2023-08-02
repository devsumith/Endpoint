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


[Verb("ctrl-create")]
public class ControlCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ControlCreateRequestHandler : IRequestHandler<ControlCreateRequest>
{
    private readonly ILogger<ControlCreateRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public ControlCreateRequestHandler(
        ILogger<ControlCreateRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(ControlCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ControlCreateRequestHandler));

        _angularService.ControlCreate(request.Name, request.Directory);
    }
}
