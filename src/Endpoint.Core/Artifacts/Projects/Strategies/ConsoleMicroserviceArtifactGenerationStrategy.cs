// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Internals;
using Endpoint.Core.Services;
using MediatR;

namespace Endpoint.Core.Artifacts.Projects.Strategies;

public class ConsoleMicroserviceArtifactGenerationStrategy : GenericArtifactGenerationStrategy<ConsoleMicroserviceProjectModel>
{
    private readonly IFileSystem fileSytem;
    private readonly Observable<INotification> notificationListener;

    public ConsoleMicroserviceArtifactGenerationStrategy(

        IFileSystem fileSystem,
        Observable<INotification> notificationListener)
    {
        fileSytem = fileSystem;
        this.notificationListener = notificationListener;
    }

    public override async Task GenerateAsync(IArtifactGenerator artifactGenerator, ConsoleMicroserviceProjectModel model)
    {
        throw new NotImplementedException();
    }
}
