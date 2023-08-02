// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Artifacts.Projects.Services;

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
    private readonly ILogger<ControllerMethodAddRequestHandler> _logger;
    private readonly IApiProjectService _apiProjectService;

    public ControllerMethodAddRequestHandler(
        ILogger<ControllerMethodAddRequestHandler> logger,
        IApiProjectService apiProjectService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _apiProjectService = apiProjectService ?? throw new ArgumentNullException(nameof(apiProjectService));
    }

    public async Task Handle(ControllerMethodAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ControllerMethodAddRequestHandler));

        _apiProjectService.ControllerMethodAdd(request.Name, request.Controller, request.Route, request.Directory);
    }
}
