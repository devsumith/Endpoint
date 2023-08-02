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

[Verb("enable-default-standalone-components")]
public class EnableDefaultStandaloneComponentsRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class EnableDefaultStandaloneComponentsRequestHandler : IRequestHandler<EnableDefaultStandaloneComponentsRequest>
{
    private readonly ILogger<EnableDefaultStandaloneComponentsRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public EnableDefaultStandaloneComponentsRequestHandler(
        ILogger<EnableDefaultStandaloneComponentsRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(EnableDefaultStandaloneComponentsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(EnableDefaultStandaloneComponentsRequestHandler));

        await _angularService.EnableDefaultStandalone(new AngularProjectReferenceModel(request.Name, request.Directory));


    }
}
