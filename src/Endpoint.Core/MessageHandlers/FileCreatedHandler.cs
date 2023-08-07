// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Commands;
using Endpoint.Core.Messages;
using MediatR;
using System.Threading;

namespace Endpoint.Core.MessageHandlers;

public class FileCreatedHandler : INotificationHandler<FileCreated>
{
    private readonly IArtifactGenerator _artifactGenerator;
    public FileCreatedHandler(IArtifactGenerator artifactGenerator)
    {
        _artifactGenerator = artifactGenerator;
    }

    public async Task Handle(FileCreated notification, CancellationToken cancellationToken)
    {
        var model = new FileReferenceModel() { Path = notification.Path };

        await _artifactGenerator.GenerateAsync(model, new CopyrightAdd());
    }
}
