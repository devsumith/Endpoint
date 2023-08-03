// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("lit-workspace-create")]
public class LitWorkspaceCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class LitWorkspaceCreateRequestHandler : IRequestHandler<LitWorkspaceCreateRequest>
{
    private readonly ILogger<LitWorkspaceCreateRequestHandler> _logger;
    private readonly ICommandService _commandService;
    private readonly ILitService _litService;

    public LitWorkspaceCreateRequestHandler(
        ILogger<LitWorkspaceCreateRequestHandler> logger,
        ICommandService commandService,
        ILitService litService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _litService = litService ?? throw new ArgumentNullException(nameof(litService));
    }

    public async Task Handle(LitWorkspaceCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(LitWorkspaceCreateRequestHandler));

        await _litService.WorkspaceCreate(request.Name, request.Directory);

        _commandService.Start("code .", $"{request.Directory}{Path.DirectorySeparatorChar}{request.Name}");
    }
}
