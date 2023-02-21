// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.Artifacts.Folders.Services;

namespace Endpoint.Cli.Commands;


[Verb("aggregate-queries-folder-create")]
public class AggregateQueriesFolderCreateRequest : IRequest
{
    [Option('n', "name")]
    public string AggregateName { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AggregateQueriesFolderCreateRequestHandler : IRequestHandler<AggregateQueriesFolderCreateRequest>
{
    private readonly ILogger<AggregateQueriesFolderCreateRequestHandler> _logger;
    private readonly IFolderService _folderService;

    public AggregateQueriesFolderCreateRequestHandler(
        ILogger<AggregateQueriesFolderCreateRequestHandler> logger,
        IFolderService folderService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _folderService = folderService ?? throw new ArgumentNullException(nameof(folderService));
    }

    public async Task Handle(AggregateQueriesFolderCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AggregateQueriesFolderCreateRequestHandler));

        _folderService.AggregateQueries(request.AggregateName, request.Directory);


    }
}
