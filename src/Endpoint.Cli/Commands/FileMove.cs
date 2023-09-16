// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("file-move")]
public class FileMoveRequest : IRequest
{
    [Option('s', "searchPattern")]
    public string SearchPattern { get; set; }

    [Option('f', Required = true)]
    public string Destination { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class FileMoveRequestHandler : IRequestHandler<FileMoveRequest>
{
    private readonly ILogger<FileMoveRequestHandler> logger;

    public FileMoveRequestHandler(ILogger<FileMoveRequestHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(FileMoveRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(FileMoveRequestHandler));

        foreach (var path in Directory.GetFiles(request.Directory, request.SearchPattern, SearchOption.AllDirectories))
        {
            var destination = Path.Combine(request.Destination, Path.GetFileName(path));

            File.Move(path, destination);
        }
    }
}
