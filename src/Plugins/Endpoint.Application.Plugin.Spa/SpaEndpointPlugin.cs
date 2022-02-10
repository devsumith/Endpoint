using Endpoint.Core.Events;
using Endpoint.Core.Models;
using Endpoint.Core.Services;
using MediatR;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Plugin.Spa
{
    public class SpaEndpointPlugin : INotificationHandler<SolutionTemplateGenerated>
    {
        private readonly ICommandService _commandService;
        private readonly IFileSystem _fileSystem;
        private readonly ISettingsProvider _settingsProvider;
        public SpaEndpointPlugin(ICommandService commandService, IFileSystem fileSystem, ISettingsProvider settingsProvider)
        {
            _commandService = commandService;
            _fileSystem = fileSystem;
            _settingsProvider = settingsProvider;
        }

        public Task Handle(SolutionTemplateGenerated notification, CancellationToken cancellationToken)
        {
            var parts = notification.RootDirectory.Split(Path.DirectorySeparatorChar);

            var parent = string.Join(Path.DirectorySeparatorChar, parts.Take(parts.Length - 1));

            Settings settings = _settingsProvider.Get(notification.RootDirectory);

            var appDirectory = $"{settings.RootDirectory}{Path.DirectorySeparatorChar}{settings.SourceFolder}{Path.DirectorySeparatorChar}{settings.SolutionName}.App";

            _commandService.Start($"spa -n {settings.SolutionName}", parent);

            settings.AddApp(appDirectory, _fileSystem);

            return Task.CompletedTask;
        }

    }
}