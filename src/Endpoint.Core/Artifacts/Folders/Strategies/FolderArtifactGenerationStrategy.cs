// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Folders.Strategies;

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

    public override void Create(IArtifactGenerator artifactGenerator, FolderModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        _fileSystem.CreateDirectory(model.Directory);

        foreach (var fileModel in model.Files)
        {
            artifactGenerator.CreateFor(fileModel, context);
        }

        foreach (var folder in model.SubFolders)
        {
            artifactGenerator.CreateFor(folder, context);
        }
    }
}