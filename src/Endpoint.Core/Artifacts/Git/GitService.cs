// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using Octokit;
using Octokit.Internal;

namespace Endpoint.Core.Artifacts.Git;

using Repository = LibGit2Sharp.Repository;

public class GitService : IGitService
{
    private readonly ILogger<GitService> logger;
    private readonly ICommandService commandService;
    private readonly IFileProvider fileProvider;
    private readonly IFileSystem fileSystem;

    public GitService(ILogger<GitService> logger, ICommandService commandService, IFileProvider fileProvider, IFileSystem fileSystem)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public async Task CreatePullRequestAsync(string pullRequestTitle, string directory)
    {
        try
        {
            logger.LogInformation("Creating Pull Request");

            var pathToGitFolder = fileProvider.Get("*.gitignore", directory);

            var repositoryName = "Endpoint";

            var model = new GitModel(repositoryName)
            {
                Directory = fileSystem.Path.GetDirectoryName(pathToGitFolder), // figure out by direcory. Location of .git folder
            };

            var featureBranchName = GetCurrentBranch(fileSystem.Path.GetDirectoryName(pathToGitFolder));

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

            commandService.Start("git checkout master", directory);

            commandService.Start("git pull", directory);
        }catch(Exception ex)
        {
            throw ex;
        }
    }

    public string GetCurrentBranch(string repositoryPath)
    {
        using (var repo = new Repository(repositoryPath))
        {
            var branch = repo.Head;
            return branch.FriendlyName;
        }
    }
}
