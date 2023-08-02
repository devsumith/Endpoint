// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts.Projects.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("controller-create")]
public class ControllerCreateRequest : IRequest
{
    [Option('n')]
    public string EntityName { get; set; }

    [Option('e', "empty")]
    public bool Empty { get; set; }

    [Option('d')]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ControllerCreateRequestHandler : IRequestHandler<ControllerCreateRequest>
{
    private readonly ILogger<ControllerCreateRequestHandler> _logger;
    private readonly IApiProjectService _apiProjectService;

    public ControllerCreateRequestHandler(ILogger<ControllerCreateRequestHandler> logger, IApiProjectService apiProjectService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _apiProjectService = apiProjectService ?? throw new ArgumentNullException(nameof(apiProjectService));
    }

    public async Task Handle(ControllerCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ControllerCreateRequestHandler));

        _apiProjectService.ControllerAdd(request.EntityName, request.Empty, request.Directory);
    }
}

