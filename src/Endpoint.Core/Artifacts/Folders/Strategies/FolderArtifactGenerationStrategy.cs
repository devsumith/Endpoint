// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Folders.Strategies;

public class FolderArtifactGenerationStrategy : GenericArtifactGenerationStrategy<FolderModel>
{
    private readonly ILogger<FolderArtifactGenerationStrategy> _logger;
    private readonly IFileSystem _fileSystem;
    public FolderArtifactGenerationStrategy(
        IFileSystem fileSystem,
        ILogger<FolderArtifactGenerationStrategy> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public override async Task GenerateAsync(IArtifactGenerator artifactGenerator, FolderModel model)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        _fileSystem.Directory.CreateDirectory(model.Directory);

        foreach (var fileModel in model.Files)
        {
            await artifactGenerator.GenerateAsync(fileModel);
        }

        foreach (var folder in model.SubFolders)
        {
            await artifactGenerator.GenerateAsync(folder);
        }
    }
}
