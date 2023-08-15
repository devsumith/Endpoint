// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using Octokit;
using System.IO;

namespace Endpoint.Core.Artifacts.Git;

public class GitGenerationStrategy : IArtifactGenerationStrategy<GitModel>
{
    private readonly ICommandService _commandService;
    private readonly ILogger<GitGenerationStrategy> _logger;
    private readonly ITemplateLocator _templateLocator;
    private readonly IFileSystem _fileSystem;

    public GitGenerationStrategy(IServiceProvider serviceProvider, ILogger<GitGenerationStrategy> logger, ICommandService commandService, ITemplateLocator templateLocator, IFileSystem fileSystem)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public int Priority { get; set; } = 0;

    public async Task GenerateAsync(IArtifactGenerator artifactGenerator, GitModel model, dynamic context = null)
    {
        _logger.LogInformation($"{nameof(GitGenerationStrategy)}: Handled");

        var client = new GitHubClient(new ProductHeaderValue(model.Username))
        {
            Credentials = new Credentials(model.PersonalAccessToken)
        };

        client.Repository.Create(new NewRepository(model.RepositoryName)).GetAwaiter().GetResult();

        _commandService.Start($"git init", $@"{model.Directory}");

        _commandService.Start($"git config user.name {model.Username}", model.Directory);

        _commandService.Start($"git config user.email {model.Email}", model.Directory);

        _fileSystem.File.WriteAllText($@"{model.Directory}{Path.DirectorySeparatorChar}.gitignore", string.Join(Environment.NewLine, _templateLocator.Get("GitIgnoreFile")));

        _commandService.Start($"git remote add origin https://{model.Username}:{model.PersonalAccessToken}@github.com/{model.Username}/{model.RepositoryName}.git", model.Directory);

        _commandService.Start("git add -A", model.Directory);

        _commandService.Start("git commit -m intial", model.Directory);

        _commandService.Start("git push --set-upstream origin master", model.Directory);
    }

}

