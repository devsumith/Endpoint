using CommandLine;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Commands
{
    internal class Git
    {
        [Verb("git")]
        internal class Request : IRequest<Unit>
        {
            [Value(0)]
            public string RepositoryName { get; set; }

            [Option('d', Required = false)]
            public string Directory { get; set; } = Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
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

                var username = _configuration["GitHubUsername"];

                if(string.IsNullOrEmpty(username))
                {
                    Console.WriteLine("Please enter your github username:");
                    username = Console.ReadLine();
                    _addOrUpdateAppSettingSettingsEntry("GitHubUsername", username);
                }

                var email = _configuration["GitHubEmail"];

                if (string.IsNullOrEmpty(email))
                {
                    Console.WriteLine("Please enter your github email:");
                    email = Console.ReadLine();

                    _addOrUpdateAppSettingSettingsEntry("GitHubEmail", username);
                }

                var personalAccessToken = _configuration["GitHubPersonalAccessToken"];

                if (string.IsNullOrEmpty(personalAccessToken))
                {
                    Console.WriteLine("Please enter your github personal access token:");
                    personalAccessToken = Console.ReadLine();
                    _addOrUpdateAppSettingSettingsEntry("GitHubPersonalAccessToken", username);
                }

                var client = new GitHubClient(new ProductHeaderValue(username))
                {
                    Credentials = new Credentials(personalAccessToken)
                };

                client.Repository.Create(new NewRepository(request.RepositoryName)).GetAwaiter().GetResult();

                _commandService.Start($"git init", $@"{request.Directory}");

                _commandService.Start($"git config user.name {username}", request.Directory);

                _commandService.Start($"git config user.email {email}", request.Directory);

                _fileSystem.WriteAllLines($@"{request.Directory}\.gitignore", _templateLocator.Get("GitIgnoreFile"));

                _commandService.Start($"git remote add origin https://{username}:{personalAccessToken}@github.com/{username}/{request.RepositoryName}.git");

                return new();
            }

            private void _addOrUpdateAppSettingSettingsEntry(string key, string value)
            {
                var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                var json = File.ReadAllText(appSettingsPath);
                var jObject = JsonConvert.DeserializeObject<JObject>(json);

                if (jObject.GetValue(key) == null)
                {
                    jObject.Add(key, value);
                }
                else
                {
                    jObject[key] = value;
                }

                File.WriteAllText(appSettingsPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
            }
        }
    }
}
