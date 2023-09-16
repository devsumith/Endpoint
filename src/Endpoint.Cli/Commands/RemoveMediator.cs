// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("remove-mediator")]
public class RemoveMediatorRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class RemoveMediatorRequestHandler : IRequestHandler<RemoveMediatorRequest>
{
    private readonly ILogger<RemoveMediatorRequestHandler> logger;
    private readonly ICommandService commandService;

    public RemoveMediatorRequestHandler(
        ILogger<RemoveMediatorRequestHandler> logger,
        ICommandService commandService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(RemoveMediatorRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(RemoveMediatorRequestHandler));

        foreach (var file in Directory.GetFiles(request.Directory, "*.Core.csproj", SearchOption.AllDirectories))
        {
            commandService.Start("dotnet remove package MediatR", Path.GetDirectoryName(file));

            commandService.Start("dotnet remove package MediatR.Extensions.Microsoft.DependencyInjection", Path.GetDirectoryName(file));
        }
    }
}
