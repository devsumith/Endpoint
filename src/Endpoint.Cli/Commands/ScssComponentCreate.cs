// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using Endpoint.Core.WebArtifacts.Services;

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
    private readonly ILogger<ScssComponentCreateRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public ScssComponentCreateRequestHandler(
        ILogger<ScssComponentCreateRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(ScssComponentCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ScssComponentCreateRequestHandler));

        await _angularService.ScssComponentCreate(request.Name, request.Directory);


    }
}
