// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Projects.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("controller-method-add")]
public class ControllerMethodAddRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('c', "controller")]
    public string Controller { get; set; } = "Default";

    [Option('r', "route")]
    public string Route { get; set; } = "httpget";

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ControllerMethodAddRequestHandler : IRequestHandler<ControllerMethodAddRequest>
{
    private readonly ILogger<ControllerMethodAddRequestHandler> logger;
    private readonly IApiProjectService apiProjectService;

    public ControllerMethodAddRequestHandler(
        ILogger<ControllerMethodAddRequestHandler> logger,
        IApiProjectService apiProjectService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.apiProjectService = apiProjectService ?? throw new ArgumentNullException(nameof(apiProjectService));
    }

    public async Task Handle(ControllerMethodAddRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(ControllerMethodAddRequestHandler));

        await apiProjectService.ControllerMethodAdd(request.Name, request.Controller, request.Route, request.Directory);
    }
}
