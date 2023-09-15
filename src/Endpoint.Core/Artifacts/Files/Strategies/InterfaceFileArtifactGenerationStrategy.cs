// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Interfaces;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class InterfaceFileArtifactGenerationStrategy : CodeFileIArtifactGenerationStrategy<InterfaceModel>
{
    public InterfaceFileArtifactGenerationStrategy(ISyntaxGenerator syntaxGenerator, IFileSystem fileSystem, INamespaceProvider namespaceProvider, IGenericArtifactGenerationStrategy<FileModel> fileArtifactGenerationStrategy, ILogger<CodeFileIArtifactGenerationStrategy<InterfaceModel>> logger) 
        : base(syntaxGenerator, fileSystem, namespaceProvider, fileArtifactGenerationStrategy, logger)
    {
    }
}
