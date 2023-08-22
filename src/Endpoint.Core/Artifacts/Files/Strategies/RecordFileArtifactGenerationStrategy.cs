// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Internals;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Record;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class RecordFileArtifactGenerationStrategy : ObjectFileIArtifactGenerationStrategy<RecordModel>
{
    public RecordFileArtifactGenerationStrategy(ISyntaxGenerator syntaxGenerator, IFileSystem fileSystem, INamespaceProvider namespaceProvider, Observable<INotification> notificationListener, IGenericArtifactGenerationStrategy<FileModel> fileArtifactGenerationStrategy, ILogger<ObjectFileIArtifactGenerationStrategy<RecordModel>> logger) 
        : base(syntaxGenerator, fileSystem, namespaceProvider, notificationListener, fileArtifactGenerationStrategy, logger)
    {
    }
}
