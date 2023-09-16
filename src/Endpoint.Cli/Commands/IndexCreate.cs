// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb(".")]
public class IndexCreateRequest : IRequest
{
    [Option('s', "scss")]
    public bool Scss { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class IndexCreateRequestHandler : IRequestHandler<IndexCreateRequest>
{
    private readonly ILogger<IndexCreateRequestHandler> logger;
    private readonly IFileSystem fileSystem;
    private readonly IAngularService angularService;

    public IndexCreateRequestHandler(
        ILogger<IndexCreateRequestHandler> logger,
        IFileSystem fileSystem,
        IAngularService angularService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(IndexCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(IndexCreateRequestHandler));

        await angularService.IndexCreate(request.Scss, request.Directory);
    }
}
