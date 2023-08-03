// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts.Folders.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;

[Verb("aggregate-commands-folder-create")]
public class AggregateCommandsFolderCreateRequest : IRequest
{
    [Option('n', "name")]
    public string AggregateName { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AggregateCommandsFolderCreateRequestHandler : IRequestHandler<AggregateCommandsFolderCreateRequest>
{
    private readonly ILogger<AggregateCommandsFolderCreateRequestHandler> _logger;
    private readonly IFolderService _folderService;

    public AggregateCommandsFolderCreateRequestHandler(
        ILogger<AggregateCommandsFolderCreateRequestHandler> logger,
        IFolderService folderService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _folderService = folderService ?? throw new ArgumentNullException(nameof(folderService));
    }

    public async Task Handle(AggregateCommandsFolderCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AggregateCommandsFolderCreateRequestHandler));

        await _folderService.AggregateCommands(new(request.AggregateName), request.Directory);
    }
}
