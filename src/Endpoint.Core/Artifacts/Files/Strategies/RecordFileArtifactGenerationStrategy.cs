// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Records;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class RecordFileArtifactGenerationStrategy : CodeFileIArtifactGenerationStrategy<RecordModel>
{
    public RecordFileArtifactGenerationStrategy(ISyntaxGenerator syntaxGenerator, IFileSystem fileSystem, INamespaceProvider namespaceProvider, IGenericArtifactGenerationStrategy<FileModel> fileArtifactGenerationStrategy, ILogger<CodeFileIArtifactGenerationStrategy<RecordModel>> logger)
        : base(syntaxGenerator, fileSystem, namespaceProvider, fileArtifactGenerationStrategy, logger)
    {
    }
}
