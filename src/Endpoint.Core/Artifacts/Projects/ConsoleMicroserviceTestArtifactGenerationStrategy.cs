// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;

namespace Endpoint.Core.Artifacts.Projects;

public class ConsoleMicroserviceTestArtifactGenerationStrategy : IArtifactGenerationStrategy<ConsoleMicroserviceProjectModel>
{
    private readonly IFileSystem _fileSytem;
    public ConsoleMicroserviceTestArtifactGenerationStrategy(IServiceProvider serviceProvider, IFileSystem fileSystem)

    {
        _fileSytem = fileSystem;
    }

    public int Priority => 0;


    public async Task GenerateAsync(IArtifactGenerator artifactGenerator, ConsoleMicroserviceProjectModel model, dynamic context = null)
    {
        throw new NotImplementedException();
    }
}
