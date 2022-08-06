using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using Octokit;
using System;
using System.IO;

namespace Endpoint.Core.Strategies.Common.Git
{
    public class GitGenerationStrategy: IGitGenerationStrategy
    {
        private readonly ICommandService _commandService;
        private readonly ILogger _logger;
        private readonly ITemplateLocator _templateLocator;
        private readonly IFileSystem _fileSystem;

        public GitGenerationStrategy(ILogger logger, ICommandService commandService, ITemplateLocator templateLocator, IFileSystem fileSystem)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }
        public bool CanHandle(GitModel model) => true;

        public void Create(GitModel model)
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

            _fileSystem.WriteAllLines($@"{model.Directory}{Path.DirectorySeparatorChar}.gitignore", _templateLocator.Get("GitIgnoreFile"));

            _commandService.Start($"git remote add origin https://{model.Username}:{model.PersonalAccessToken}@github.com/{model.Username}/{model.RepositoryName}.git", model.Directory);

            _commandService.Start("git add -A", model.Directory);

            _commandService.Start("git commit -m intial", model.Directory);

            _commandService.Start("git push --set-upstream origin master", model.Directory);
        }

        public int Order { get; set; } = 0;
    }
}
