// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.



using Microsoft.Extensions.Logging;
using Octokit;

namespace Endpoint.Core.Artifacts.Git;

public class GitService : IGitService
{
    private readonly ILogger<GitService> logger;

    public GitService(ILogger<GitService> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task CreatePullRequestAsync(GitModel model, string featureBranchName, string pullRequestTitle)
    {
        logger.LogInformation("Creating Pull Request");

        var client = new GitHubClient(new ProductHeaderValue(model.Username))
        {
            Credentials = new Credentials(model.PersonalAccessToken),
        };

        var repo = await client.Repository.Get(model.Username, model.RepositoryName);

        var defaultBranch = await client.Git.Reference.Get(model.Username, model.RepositoryName, "refs/heads/master");

        var featureBranch = await client.Git.Reference.Get(model.Username, model.RepositoryName, $@"refs/heads/{featureBranchName}");

        _ = await client.PullRequest.Create(repo.Id, new NewPullRequest(pullRequestTitle, featureBranch.Ref, defaultBranch.Ref));

        var newMerge = new NewMerge(defaultBranch.Ref, featureBranch.Ref);

        await client.Repository.Merging.Create(repo.Id, newMerge);
    }
}
