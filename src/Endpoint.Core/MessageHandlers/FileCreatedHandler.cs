// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Messages;
using Endpoint.Core.Models.Artifacts.Files;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Core.MessageHandlers;

public class FileCreatedHandler : INotificationHandler<FileCreated>
{
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    public FileCreatedHandler(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory)
    {
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory;
    }

    public async Task Handle(FileCreated notification, CancellationToken cancellationToken)
    {
        var model = new FileReferenceModel() { Path = notification.Path };

        _artifactGenerationStrategyFactory.CreateFor(model, new Models.Artifacts.Files.Commands.CopyrightAdd());
    }
}
