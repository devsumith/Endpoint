// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class ContentFileArtifactGenerationStrategy : IArtifactGenerationStrategy<ContentFileModel>
{
    private readonly IFileSystem _fileSystem;
    public ContentFileArtifactGenerationStrategy(
        IServiceProvider serviceProvider,
        IFileSystem fileSystem
        )
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public int Priority => 0;
    public async Task GenerateAsync(IArtifactGenerator artifactGenerator, ContentFileModel model, dynamic context = null)
    {
        _fileSystem.WriteAllText(model.Path, model.Content);
    }
}
