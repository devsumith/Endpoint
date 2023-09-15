// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Namespaces;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class NamespaceFileArtifactGenerationStrategy : CodeFileIArtifactGenerationStrategy<NamespaceModel>
{
    public NamespaceFileArtifactGenerationStrategy(ISyntaxGenerator syntaxGenerator, IFileSystem fileSystem, INamespaceProvider namespaceProvider, IGenericArtifactGenerationStrategy<FileModel> fileArtifactGenerationStrategy, ILogger<CodeFileIArtifactGenerationStrategy<NamespaceModel>> logger)
        : base(syntaxGenerator, fileSystem, namespaceProvider, fileArtifactGenerationStrategy, logger)
    {
    }
}