// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Messages;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Files.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

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

        _artifactGenerator.CreateFor(model, new CopyrightAdd());
    }
}
