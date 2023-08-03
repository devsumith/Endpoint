// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Services;

namespace Endpoint.Cli.Commands;


[Verb("gitignore-create")]
public class GitignoreCreateRequest : IRequest {
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class GitignoreCreateRequestHandler : IRequestHandler<GitignoreCreateRequest>
{
    private readonly ILogger<GitignoreCreateRequestHandler> _logger;
    private readonly ICommandService _commandService;

    public GitignoreCreateRequestHandler(
        ILogger<GitignoreCreateRequestHandler> logger,
        ICommandService commandService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(GitignoreCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Generating Gitignore file.");

        _commandService.Start("dotnet new gitignore", request.Directory);
    }
}