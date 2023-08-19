﻿// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Internals;
using Endpoint.Core.Services;
using MediatR;

namespace Endpoint.Core.Artifacts.Projects.Strategies;

public class ConsoleMicroserviceArtifactGenerationStrategy : GenericArtifactGenerationStrategy<ConsoleMicroserviceProjectModel>
{
    private readonly IFileSystem _fileSytem;
    private readonly Observable<INotification> _notificationListener;
    public ConsoleMicroserviceArtifactGenerationStrategy(
        IServiceProvider serviceProvider,
        IFileSystem fileSystem,
        Observable<INotification> notificationListener)

    {
        _fileSytem = fileSystem;
        _notificationListener = notificationListener;
    }



    public override async Task GenerateAsync(IArtifactGenerator artifactGenerator, ConsoleMicroserviceProjectModel model, dynamic context = null)
    {
        throw new NotImplementedException();
    }
}