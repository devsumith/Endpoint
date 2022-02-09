using Endpoint.Application.Core.Events;
using Endpoint.Application.Services;
using Endpoint.SharedKernal.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Plugin.Identity
{
    public class IdentityEndpointPlugin : INotificationHandler<SolutionTemplateGenerated>
    {
        private readonly IApplicationFileService _applicationFileService;
        private readonly IFileSystem _fileSystem;
        private readonly ISettingsProvider _settingsProvider;
        private readonly IInfrastructureFileService _infrastructureFileService;
        private readonly IApiFileService _apiFileService;
        public IdentityEndpointPlugin(
            IApplicationFileService applicationFileService,
            IFileSystem fileSystem,
            ISettingsProvider settingsProvider,
            IInfrastructureFileService infrastructureFileService,
            IApiFileService apiFileService)
        {
            _applicationFileService = applicationFileService;
            _fileSystem = fileSystem;
            _settingsProvider = settingsProvider;
            _infrastructureFileService = infrastructureFileService;
            _apiFileService = apiFileService;
        }

        public Task Handle(SolutionTemplateGenerated notification, CancellationToken cancellationToken)
        {
            var settings = _settingsProvider.Get(notification.RootDirectory);

            settings.AddResource("User", _fileSystem);

            settings.AddResource("Role", _fileSystem);

            // Add User to Resources in Settings

            //--------------- Shared Kernal
            /*
             *  1. Authentication Folder
             *      a. Authentication (Config Options) File
             *      b. Token Provider
             *      c. Token Builder
             *      d. Password Hasher
             *      e. ResourceOperationAuthorizationHandler
             *     
             *  2. Add claimTypes to Contants File (UserId, Username)   
             * 
             */

            //--------------- Application
            /*
             *  1. User Aggregate
             *  2. Authenticate Command
             *  3. Get Current User Query
             *  
             * 
             */

            _applicationFileService.BuildAdditionalResource("User", settings);
            _applicationFileService.BuildAdditionalResource("Role", settings);

            //--------------- Infrastructure
            /*
             *  1. Update DbContext 
             *  2. Seed Default User
             */

            _infrastructureFileService.BuildAdditionalResource(null, settings);

            //--------------- Api
            /*
             *  1. Add User Api Controller with Authenticate and GetCurrent User 
             * 
             */

            _apiFileService.BuildAdditionalResource("User", settings);
            _apiFileService.BuildAdditionalResource("Role", settings);

            return Task.CompletedTask;
        }

    }
}
