using Endpoint.Application.Core.Events;
using Endpoint.Application.Services;
using Endpoint.SharedKernal.Models;
using Endpoint.SharedKernal.Services;
using MediatR;
using System.IO;
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
            Settings settings = _settingsProvider.Get(notification.RootDirectory);

            var appDirectory = $"{settings.RootDirectory}{Path.DirectorySeparatorChar}{settings.SourceFolder}{Path.DirectorySeparatorChar}{settings.SolutionName}.App";

            settings.AddApp(appDirectory, _fileSystem);

            _commandService.Start($"spa -n {settings.SolutionName}", settings.RootDirectory);

            return Task.CompletedTask;
        }

    }
}
