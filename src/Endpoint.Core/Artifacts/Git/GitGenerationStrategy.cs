// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using Octokit;

namespace Endpoint.Core.Artifacts.Git;

public class GitGenerationStrategy : GenericArtifactGenerationStrategy<GitModel>
{
    private readonly ICommandService commandService;
    private readonly ILogger<GitGenerationStrategy> logger;
    private readonly ITemplateLocator templateLocator;
    private readonly IFileSystem fileSystem;

    public GitGenerationStrategy(ILogger<GitGenerationStrategy> logger, ICommandService commandService, ITemplateLocator templateLocator, IFileSystem fileSystem)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        this.templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public override async Task GenerateAsync(IArtifactGenerator artifactGenerator, GitModel model)
    {
        logger.LogInformation($"{nameof(GitGenerationStrategy)}: Handled");

        var client = new GitHubClient(new ProductHeaderValue(model.Username))
        {
            Credentials = new Credentials(model.PersonalAccessToken),
        };

        client.Repository.Create(new NewRepository(model.RepositoryName)).GetAwaiter().GetResult();

        commandService.Start($"git init", $@"{model.Directory}");

        commandService.Start($"git config user.name {model.Username}", model.Directory);

        commandService.Start($"git config user.email {model.Email}", model.Directory);

        fileSystem.File.WriteAllText($@"{model.Directory}{Path.DirectorySeparatorChar}.gitignore", string.Join(Environment.NewLine, templateLocator.Get("GitIgnoreFile")));

        commandService.Start($"git remote add origin https://{model.Username}:{model.PersonalAccessToken}@github.com/{model.Username}/{model.RepositoryName}.git", model.Directory);

        commandService.Start("git add -A", model.Directory);

        commandService.Start("git commit -m intial", model.Directory);

        commandService.Start("git push --set-upstream origin master", model.Directory);

        commandService.Start("git checkout -b gh-pages", model.Directory);

        commandService.Start("git push --set-upstream origin gh-pages", model.Directory);

        commandService.Start("git checkout -", model.Directory);
    }
}
