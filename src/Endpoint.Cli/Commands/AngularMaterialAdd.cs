// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.WebArtifacts.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("ng-material-add")]
public class AngularMaterialAddRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularMaterialAddRequestHandler : IRequestHandler<AngularMaterialAddRequest>
{
    private readonly ILogger<AngularMaterialAddRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public AngularMaterialAddRequestHandler(
        ILogger<AngularMaterialAddRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(AngularMaterialAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AngularMaterialAddRequestHandler));

        _angularService.MaterialAdd(new(request.Name, request.Directory));
    }
}
