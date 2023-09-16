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

[Verb("file-rename")]
public class FileRenameRequest : IRequest
{
    [Option('o', "old")]
    public string OldEndsWith { get; set; }

    [Option('n', "mew")]
    public string NewEndsWith { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class FileRenameRequestHandler : IRequestHandler<FileRenameRequest>
{
    private readonly ILogger<FileRenameRequestHandler> logger;

    public FileRenameRequestHandler(ILogger<FileRenameRequestHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(FileRenameRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(FileRenameRequestHandler));

        foreach (var path in Directory.GetFiles(request.Directory, $"*{request.OldEndsWith}", SearchOption.AllDirectories))
        {
            var destinationPath = path.Replace(request.OldEndsWith, request.NewEndsWith);

            File.Move(path, destinationPath);
        }
    }
}
