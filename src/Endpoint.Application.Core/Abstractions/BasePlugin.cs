using Endpoint.Application.Services;
using Endpoint.SharedKernal.Models;
using Endpoint.SharedKernal.Services;
using System;

namespace Endpoint.Application.Core.Abstractions
{
    public abstract class BasePlugin
    {
        protected readonly IApplicationFileService _applicationFileService;
        protected readonly IFileSystem _fileSystem;
        protected readonly ISettingsProvider _settingsProvider;
        protected readonly IInfrastructureFileService _infrastructureFileService;
        protected readonly IApiFileService _apiFileService;

        public BasePlugin(
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

        protected virtual void Build(
            string aggregateName,
            Settings settings,
            IFileSystem fileSystem,
            Action<Settings, IFileSystem> aggregateBuilder,
            Action<Settings, IFileSystem> controllerBuilder)
        {
            settings.AddResource(aggregateName, fileSystem);

            aggregateBuilder(settings, fileSystem);

            _applicationFileService.BuildAdditionalResource(aggregateName, settings);

            _infrastructureFileService.BuildAdditionalResource(null, settings);

            _apiFileService.BuildAdditionalResource(aggregateName, settings);

            controllerBuilder(settings, fileSystem);
        }
    }
}
