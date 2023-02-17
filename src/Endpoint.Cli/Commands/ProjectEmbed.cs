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
using System.Collections.Generic;

namespace Endpoint.Cli.Commands;


[Verb("project-embed")]
public class ProjectEmbedRequest : IRequest
{
    [Option('o', "output-directory")]
    public string OutputDirectory { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ProjectEmbedRequestHandler : IRequestHandler<ProjectEmbedRequest>
{
    private readonly ILogger<ProjectEmbedRequestHandler> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;

    public ProjectEmbedRequestHandler(
        ILogger<ProjectEmbedRequestHandler> logger,
        IFileProvider fileProvider,
        IFileSystem fileSystem)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _fileSystem = fileSystem;
    }

    public async Task Handle(ProjectEmbedRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ProjectEmbedRequestHandler));

        var projectDirectory = Path.GetDirectoryName(_fileProvider.Get("*.csproj", request.Directory));

        foreach (var path in Directory.GetFiles(projectDirectory, "*.cs", SearchOption.AllDirectories))
        {
            if (path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
                || path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"))
            {
                break;
            }

            var content = _fileSystem.ReadAllText(path);

            var resolvedPath = path.Replace(projectDirectory, request.OutputDirectory);

            resolvedPath = resolvedPath.Replace(Path.GetExtension(path), ".txt");

            List<string> parts = new List<string>();

            foreach (var part in Path.GetDirectoryName(resolvedPath).Split(Path.DirectorySeparatorChar))
            {
                parts.Add(part);

                if (parts.Count > 1)
                {
                    _fileSystem.CreateDirectory(string.Join(Path.DirectorySeparatorChar, parts));
                }
            }
            _fileSystem.WriteAllText(resolvedPath, content);
        }

    }
}
