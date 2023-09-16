// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<ProjectEmbedRequestHandler> logger;
    private readonly IFileProvider fileProvider;
    private readonly IFileSystem fileSystem;

    public ProjectEmbedRequestHandler(
        ILogger<ProjectEmbedRequestHandler> logger,
        IFileProvider fileProvider,
        IFileSystem fileSystem)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.fileSystem = fileSystem;
    }

    public async Task Handle(ProjectEmbedRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(ProjectEmbedRequestHandler));

        var projectDirectory = Path.GetDirectoryName(fileProvider.Get("*.csproj", request.Directory));

        foreach (var path in Directory.GetFiles(projectDirectory, "*.cs", SearchOption.AllDirectories))
        {
            if (path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
                || path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"))
            {
                break;
            }

            var content = fileSystem.File.ReadAllText(path);

            var resolvedPath = path.Replace(projectDirectory, request.OutputDirectory);

            resolvedPath = resolvedPath.Replace(Path.GetExtension(path), ".txt");

            List<string> parts = new List<string>();

            foreach (var part in Path.GetDirectoryName(resolvedPath).Split(Path.DirectorySeparatorChar))
            {
                parts.Add(part);

                if (parts.Count > 1)
                {
                    fileSystem.Directory.CreateDirectory(string.Join(Path.DirectorySeparatorChar, parts));
                }
            }

            fileSystem.File.WriteAllText(resolvedPath, content);
        }
    }
}
