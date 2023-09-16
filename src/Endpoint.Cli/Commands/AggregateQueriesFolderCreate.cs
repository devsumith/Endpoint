// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Folders.Services;
using Endpoint.Core.Syntax.Classes;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<AggregateQueriesFolderCreateRequestHandler> logger;
    private readonly IFolderService folderService;

    public AggregateQueriesFolderCreateRequestHandler(
        ILogger<AggregateQueriesFolderCreateRequestHandler> logger,
        IFolderService folderService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.folderService = folderService ?? throw new ArgumentNullException(nameof(folderService));
    }

    public async Task Handle(AggregateQueriesFolderCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(AggregateQueriesFolderCreateRequestHandler));

        folderService.AggregateQueries(new ClassModel(request.AggregateName), request.Directory);
    }
}
