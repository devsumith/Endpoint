// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Git;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("git-create")]
public class GitCreateRequest : IRequest
{
    [Option('n', "name")]
    public string RepositoryName { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

public class GitCreateRequestHandler : IRequestHandler<GitCreateRequest>
{
    private readonly ILogger<GitCreateRequestHandler> logger;
    private readonly IArtifactGenerator artifactGenerator;

    public GitCreateRequestHandler(
        ILogger<GitCreateRequestHandler> logger,
        IArtifactGenerator artifactGenerator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(GitCreateRequest request, CancellationToken cancellationToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Creating Git Repository");
        }

        var model = new GitModel(request.RepositoryName)
        {
            Directory = Path.Combine(request.Directory, request.RepositoryName),
        };

        await artifactGenerator.GenerateAsync(model);
    }
}
