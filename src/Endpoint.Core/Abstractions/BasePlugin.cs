using Endpoint.Core.Models;
using Endpoint.Core.Services;
using System;

namespace Endpoint.Core.Abstractions
{
    public abstract class BasePlugin
    {
        protected readonly IApplicationProjectFilesGenerationStrategy _applicationFileService;
        protected readonly IFileSystem _fileSystem;
        protected readonly ISettingsProvider _settingsProvider;
        protected readonly IInfrastructureProjectFilesGenerationStrategy _infrastructureFileService;
        protected readonly IApiProjectFilesGenerationStrategy _apiFileService;

        public BasePlugin(
            IApplicationProjectFilesGenerationStrategy applicationFileService,
            IFileSystem fileSystem,
            ISettingsProvider settingsProvider,
            IInfrastructureProjectFilesGenerationStrategy infrastructureFileService,
            IApiProjectFilesGenerationStrategy apiFileService)
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
