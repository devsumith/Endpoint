// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("lit-workspace-create")]
public class LitWorkspaceCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

public class LitWorkspaceCreateRequestHandler : IRequestHandler<LitWorkspaceCreateRequest>
{
    private readonly ILogger<LitWorkspaceCreateRequestHandler> logger;
    private readonly ICommandService commandService;
    private readonly ILitService litService;

    public LitWorkspaceCreateRequestHandler(
        ILogger<LitWorkspaceCreateRequestHandler> logger,
        ICommandService commandService,
        ILitService litService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        this.litService = litService ?? throw new ArgumentNullException(nameof(litService));
    }

    public async Task Handle(LitWorkspaceCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(LitWorkspaceCreateRequestHandler));

        await litService.WorkspaceCreate(request.Name, request.Directory);

        commandService.Start("code .", Path.Combine(request.Directory, request.Name));
    }
}
