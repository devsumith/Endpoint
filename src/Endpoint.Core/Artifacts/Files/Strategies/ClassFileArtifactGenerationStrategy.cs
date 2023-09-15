// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class ClassFileArtifactGenerationStrategy : CodeFileIArtifactGenerationStrategy<ClassModel>
{
    public ClassFileArtifactGenerationStrategy(ISyntaxGenerator syntaxGenerator, IFileSystem fileSystem, INamespaceProvider namespaceProvider, IGenericArtifactGenerationStrategy<FileModel> fileArtifactGenerationStrategy, ILogger<CodeFileIArtifactGenerationStrategy<ClassModel>> logger)
        : base(syntaxGenerator, fileSystem, namespaceProvider, fileArtifactGenerationStrategy, logger)
    {
    }
}
