// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.WebArtifacts.Services;
using Endpoint.Core.Models.WebArtifacts;

namespace Endpoint.Cli.Commands;


[Verb("bootstrap-add")]
public class BootstrapAddRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class BootstrapAddRequestHandler : IRequestHandler<BootstrapAddRequest>
{
    private readonly ILogger<BootstrapAddRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public BootstrapAddRequestHandler(
        ILogger<BootstrapAddRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(BootstrapAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(BootstrapAddRequestHandler));

        _angularService.BootstrapAdd(new AngularProjectReferenceModel(request.Name, request.Directory));


    }
}
