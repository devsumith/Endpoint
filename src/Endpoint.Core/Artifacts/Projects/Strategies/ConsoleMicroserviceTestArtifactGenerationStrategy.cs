// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;

namespace Endpoint.Core.Artifacts.Projects.Strategies;

public class ConsoleMicroserviceTestArtifactGenerationStrategy : GenericArtifactGenerationStrategy<ConsoleMicroserviceProjectModel>
{
    private readonly IFileSystem _fileSytem;
    public ConsoleMicroserviceTestArtifactGenerationStrategy(IServiceProvider serviceProvider, IFileSystem fileSystem)

    {
        _fileSytem = fileSystem;
    }

    public override async Task GenerateAsync(IArtifactGenerator artifactGenerator, ConsoleMicroserviceProjectModel model)
    {
        throw new NotImplementedException();
    }
}
