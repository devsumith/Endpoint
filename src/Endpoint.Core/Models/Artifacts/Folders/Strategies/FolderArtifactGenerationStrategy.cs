// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Models.Artifacts.Folders.Strategies;

public class FolderArtifactGenerationStrategy : ArtifactGenerationStrategyBase<FolderModel>
{
    private readonly ILogger<FolderArtifactGenerationStrategy> _logger;
    private readonly IFileSystem _fileSystem;
    public FolderArtifactGenerationStrategy(
        IServiceProvider serviceProvider,
        IFileSystem fileSystem,
        ILogger<FolderArtifactGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, FolderModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        _fileSystem.CreateDirectory(model.Directory);

        foreach (var fileModel in model.Files)
        {
            artifactGenerationStrategyFactory.CreateFor(fileModel, context);
        }
    }
}
