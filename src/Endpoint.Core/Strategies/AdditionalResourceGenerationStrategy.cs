// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Artifacts.Projects.Strategies;
using Endpoint.Core.Options;
using Endpoint.Core.Services;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Strategies;

public class AdditionalResourceGenerationStrategy : IAdditionalResourceGenerationStrategy
{
    private readonly IApplicationProjectFilesGenerationStrategy _applicationFileService;
    private readonly IInfrastructureProjectFilesGenerationStrategy _infrastructureFileService;
    private readonly IApiProjectFilesGenerationStrategy _apiFileService;
    private readonly ISettingsProvider _settingsProvider;
    private readonly IFileSystem _fileSystem;

    public AdditionalResourceGenerationStrategy(
        IApplicationProjectFilesGenerationStrategy applicationFileService,
        IInfrastructureProjectFilesGenerationStrategy infrastructureFileService,
        IApiProjectFilesGenerationStrategy apiFileService,
        ISettingsProvider settingsProvider,
        IFileSystem fileSystem)
    {
        _applicationFileService = applicationFileService;
        _infrastructureFileService = infrastructureFileService;
        _apiFileService = apiFileService;
        _settingsProvider = settingsProvider;
        _fileSystem = fileSystem;
    }

    public int Order => 0;
    public bool CanHandle(AddResourceOptions options) => true;

    public void Create(AddResourceOptions options)
    {
        var settings = _settingsProvider.Get(options.Directory);

        settings.AddResource(options.Resource, options.Properties, _fileSystem);

        _applicationFileService.BuildAdditionalResource(settings.Resources.First(x => x.Name == options.Resource), settings);

        _infrastructureFileService.BuildAdditionalResource(options.Resource, settings);

        _apiFileService.BuildAdditionalResource(options.Resource, settings);
    }
}

