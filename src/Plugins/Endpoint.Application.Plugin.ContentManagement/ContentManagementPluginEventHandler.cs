using Endpoint.Application.Core.Events;
using Endpoint.Application.Services;
using Endpoint.SharedKernal.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Plugin.ContentManagement
{
    public class ContentManagementEndpointPlugin : INotificationHandler<SolutionTemplateGenerated>
    {
        private readonly IApplicationFileService _applicationFileService;
        private readonly IFileSystem _fileSystem;
        private readonly ISettingsProvider _settingsProvider;

        public ContentManagementEndpointPlugin(IApplicationFileService applicationFileService, IFileSystem fileSystem, ISettingsProvider settingsProvider)
        {
            _fileSystem = fileSystem;
            _applicationFileService = applicationFileService;
            _settingsProvider = settingsProvider;
        }

        public Task Handle(SolutionTemplateGenerated notification, CancellationToken cancellationToken)
        {
            var settings = _settingsProvider.Get(notification.RootDirectory);

            settings.AddResource("Content", _fileSystem);

            _applicationFileService.BuildAdditionalResource("Content", settings);

            return Task.CompletedTask;
        }
    }
}
