// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Endpoint.Core.Services;
using Endpoint.Core.Models.WebArtifacts.Services;

namespace Endpoint.Cli.Commands;


[Verb(".")]
public class IndexCreateRequest : IRequest
{

    [Option('s')]
    public bool Scss { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class IndexCreateRequestHandler : IRequestHandler<IndexCreateRequest>
{
    private readonly ILogger<IndexCreateRequestHandler> _logger;
    private readonly IFileSystem _fileSystem;
    private readonly IAngularService _angularService;
    public IndexCreateRequestHandler(
        ILogger<IndexCreateRequestHandler> logger,
        IFileSystem fileSystem,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(IndexCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(IndexCreateRequestHandler));

        _angularService.IndexCreate(request.Scss, request.Directory);


    }
}
