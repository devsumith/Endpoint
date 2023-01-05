using CommandLine;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Octokit;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Commands;

public class Git
{
    [Verb("git")]
    public class Request : IRequest<Unit>
    {
        [Option('n',"name")]
        public string RepositoryName { get; set; }

        [Option('d', Required = false)]
        public string Directory { get; set; } = Environment.CurrentDirectory;
    }

    public class Handler : IRequestHandler<Request, Unit>
    {
        private readonly ICommandService _commandService;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ITemplateLocator _templateLocator;
        private readonly IFileSystem _fileSystem;

        public Handler(ILogger logger, IConfiguration configuration, ICommandService commandService, ITemplateLocator templateLocator, IFileSystem fileSystem)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handled: {nameof(Git)}");

            var username = Environment.GetEnvironmentVariable("Endpoint:GitUsername");

            var email = Environment.GetEnvironmentVariable("Endpoint:GitEmail");
            
            var password = Environment.GetEnvironmentVariable("Endpoint:GitPassword");

            var client = new GitHubClient(new ProductHeaderValue(username))
            {
                Credentials = new Credentials(password)
            };

            client.Repository.Create(new NewRepository(request.RepositoryName)).GetAwaiter().GetResult();

            _commandService.Start($"git init", $@"{request.Directory}");

            _commandService.Start($"git config user.name {username}", request.Directory);

            _commandService.Start($"git config user.email {email}", request.Directory);

            _fileSystem.WriteAllText($@"{request.Directory}{Path.DirectorySeparatorChar}.gitignore", string.Join(Environment.NewLine, _templateLocator.Get("GitIgnoreFile")));

            _commandService.Start($"git remote add origin https://{username}:{password}@github.com/{username}/{request.RepositoryName}.git");

            return new();
        }

    }
}
