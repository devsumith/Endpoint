// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class ContentFileArtifactGenerationStrategy : GenericArtifactGenerationStrategy<ContentFileModel>
{
    private readonly IFileSystem fileSystem;

    public ContentFileArtifactGenerationStrategy(

        IFileSystem fileSystem)
    {
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public override async Task GenerateAsync(IArtifactGenerator artifactGenerator, ContentFileModel model)
    {
        fileSystem.File.WriteAllText(model.Path, model.Content);
    }
}
