// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Artifacts.Git;
using System.IO;

namespace Endpoint.Cli.Commands;


[Verb("pr")]
public class GitPullRequestCreateRequest : IRequest {
    [Option('n',"name")]
    public string PullRequestTitle { get; set; }

    [Option('r',"repository-name")]
    public string RepositoryName { get; set; }

    [Option('b', "branch-name")]
    public string BrancName { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class GitPullRequestCreateRequestHandler : IRequestHandler<GitPullRequestCreateRequest>
{
    private readonly ILogger<GitPullRequestCreateRequestHandler> _logger;
    private readonly IGitService gitService;

    public GitPullRequestCreateRequestHandler(ILogger<GitPullRequestCreateRequestHandler> logger, IGitService gitService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.gitService = gitService ?? throw new ArgumentNullException(nameof(gitService));
    }

    public async Task Handle(GitPullRequestCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(GitPullRequestCreateRequestHandler));

        var model = new GitModel(request.RepositoryName)
        {
            Directory = Path.Combine(request.Directory, request.RepositoryName),
        };

        await gitService.CreatePullRequestAsync(model, request.BrancName, request.PullRequestTitle);
    }
}