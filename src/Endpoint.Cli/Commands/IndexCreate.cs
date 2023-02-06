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

namespace Endpoint.Cli.Commands;


[Verb(".")]
public class IndexCreateRequest : IRequest {

    [Option('s')]
    public bool Scss { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class IndexCreateRequestHandler : IRequestHandler<IndexCreateRequest>
{
    private readonly ILogger<IndexCreateRequestHandler> _logger;
    private readonly IFileSystem _fileSystem;
    public IndexCreateRequestHandler(ILogger<IndexCreateRequestHandler> logger, IFileSystem fileSystem)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public async Task<Unit> Handle(IndexCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(IndexCreateRequestHandler));

        List<string> lines = new();

        foreach (var path in Directory.GetDirectories(request.Directory))
        {
            var files = Directory.GetFiles(path);

            var fileNames = Directory.GetFiles(path).Select(Path.GetFileNameWithoutExtension);

            var containsIndex = Directory.GetFiles(path)
                .Select(Path.GetFileNameWithoutExtension)
                .Contains("index");

            if (!request.Scss && Directory.GetFiles(path)
                .Select(Path.GetFileNameWithoutExtension)
                .Contains("index"))
            {
                lines.Add($"export * from './{Path.GetFileNameWithoutExtension(path)}';");
            }
        }

        if (request.Scss)
        {

            foreach (var file in Directory.GetFiles(request.Directory, "*.scss"))
            {
                if (!file.EndsWith("index.scss"))
                    lines.Add($"@use './{Path.GetFileNameWithoutExtension(file)}';");
            }

            _fileSystem.WriteAllLines($"{request.Directory}{Path.DirectorySeparatorChar}index.scss", lines.ToArray());
        }
        else
        {
            foreach (var file in Directory.GetFiles(request.Directory, "*.ts"))
            {
                if (!file.Contains(".spec.") && !file.EndsWith("index.ts"))
                    lines.Add($"export * from './{Path.GetFileNameWithoutExtension(file)}';");
            }

            _fileSystem.WriteAllLines($"{request.Directory}{Path.DirectorySeparatorChar}index.ts", lines.ToArray());
        }

        return new();
    }
}
