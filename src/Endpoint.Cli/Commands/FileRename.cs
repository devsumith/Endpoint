// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Endpoint.Cli.Commands;


[Verb("file-rename")]
public class FileRenameRequest : IRequest {
    [Option('o',"old")]
    public string OldEndsWith { get; set; }

    [Option('n', "mew")]
    public string NewEndsWith { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class FileRenameRequestHandler : IRequestHandler<FileRenameRequest>
{
    private readonly ILogger<FileRenameRequestHandler> _logger;

    public FileRenameRequestHandler(ILogger<FileRenameRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(FileRenameRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(FileRenameRequestHandler));

        foreach(var path in Directory.GetFiles(request.Directory, $"*{request.OldEndsWith}",SearchOption.AllDirectories))
        {
            var destinationPath = path.Replace(request.OldEndsWith, request.NewEndsWith);

            File.Move(path,destinationPath);
        }
    }
}
