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
    private readonly ILogger<ControllerCreateRequestHandler> logger;
    private readonly IApiProjectService apiProjectService;

    public ControllerCreateRequestHandler(ILogger<ControllerCreateRequestHandler> logger, IApiProjectService apiProjectService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.apiProjectService = apiProjectService ?? throw new ArgumentNullException(nameof(apiProjectService));
    }

    public async Task Handle(ControllerCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(ControllerCreateRequestHandler));

        await apiProjectService.ControllerCreateAsync(request.EntityName, request.Empty, request.Directory);
    }
}
