using Endpoint.Application.Core.Abstractions;
using Endpoint.Application.Core.Events;
using Endpoint.Application.Plugins.ContactRequest;
using Endpoint.Application.Plugins.ContactRequest.Builders;
using Endpoint.Application.Services;
using Endpoint.SharedKernal.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Plugin.Identity
{
    public class ContactRequestEndpointPlugin : BasePlugin, INotificationHandler<SolutionTemplateGenerated>
    {
        public ContactRequestEndpointPlugin(IApplicationFileService applicationFileService, IFileSystem fileSystem, ISettingsProvider settingsProvider, IInfrastructureFileService infrastructureFileService, IApiFileService apiFileService) : base(applicationFileService, fileSystem, settingsProvider, infrastructureFileService, apiFileService)
        {
        }

        public Task Handle(SolutionTemplateGenerated notification, CancellationToken cancellationToken)
        {
            var settings = _settingsProvider.Get(notification.RootDirectory);

            Build("ContactRequest",
                settings, 
                _fileSystem, 
                ContactRequestAggregateBuilder.Build,  
                ContactRequestControllerBuilder.Build);

            return Task.CompletedTask;
        }

    }
}
