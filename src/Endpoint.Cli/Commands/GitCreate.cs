// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Git;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
    private readonly ILogger<GitCreateRequestHandler> _logger;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;

    public GitCreateRequestHandler(
        ILogger<GitCreateRequestHandler> logger,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
    }

    public async Task Handle(GitCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handled: {nameof(GitCreateRequestHandler)}");

        var model = new GitModel(request.RepositoryName)
        {
            Directory = $"{request.Directory}{Path.DirectorySeparatorChar}{request.RepositoryName}"
        };

        _artifactGenerationStrategyFactory.CreateFor(model);


    }
}

