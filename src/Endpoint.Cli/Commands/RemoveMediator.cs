// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Services;
using System.IO;

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
    private readonly ILogger<RemoveMediatorRequestHandler> _logger;
    private readonly ICommandService _commandService;

    public RemoveMediatorRequestHandler(
        ILogger<RemoveMediatorRequestHandler> logger,
        ICommandService commandService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(RemoveMediatorRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(RemoveMediatorRequestHandler));

        foreach (var file in Directory.GetFiles(request.Directory, "*.Core.csproj", SearchOption.AllDirectories))
        {
            _commandService.Start("dotnet remove package MediatR", Path.GetDirectoryName(file));

            _commandService.Start("dotnet remove package MediatR.Extensions.Microsoft.DependencyInjection", Path.GetDirectoryName(file));
        }
    }
}
