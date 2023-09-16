// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("gitignore-create")]
public class GitignoreCreateRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class GitignoreCreateRequestHandler : IRequestHandler<GitignoreCreateRequest>
{
    private readonly ILogger<GitignoreCreateRequestHandler> logger;
    private readonly ICommandService commandService;

    public GitignoreCreateRequestHandler(
        ILogger<GitignoreCreateRequestHandler> logger,
        ICommandService commandService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(GitignoreCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Generating Gitignore file.");

        commandService.Start("dotnet new gitignore", request.Directory);
    }
}