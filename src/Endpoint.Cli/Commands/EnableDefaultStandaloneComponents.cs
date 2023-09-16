// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.AngularProjects;
using Endpoint.Core.Artifacts.Services;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<EnableDefaultStandaloneComponentsRequestHandler> logger;
    private readonly IAngularService angularService;

    public EnableDefaultStandaloneComponentsRequestHandler(
        ILogger<EnableDefaultStandaloneComponentsRequestHandler> logger,
        IAngularService angularService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(EnableDefaultStandaloneComponentsRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(EnableDefaultStandaloneComponentsRequestHandler));

        await angularService.EnableDefaultStandalone(new AngularProjectReferenceModel(request.Name, request.Directory));
    }
}
