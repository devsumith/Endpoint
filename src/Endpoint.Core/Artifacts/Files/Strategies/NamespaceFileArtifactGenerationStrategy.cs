// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Text;
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

    public override async Task GenerateAsync(IArtifactGenerator generator, CodeFileModel<NamespaceModel> model)
    {
        logger.LogInformation("Generating Code File. {name}", model.Name);

        var stringBuilder = new StringBuilder();

        foreach (var @using in model.Usings)
        {
            stringBuilder.AppendLine($"using {@using.Name};");

            if (@using == model.Usings.Last())
            {
                stringBuilder.AppendLine();
            }
        }

        stringBuilder.AppendLine();

        stringBuilder.AppendLine(await syntaxGenerator.GenerateAsync(model.Object));

        model.Body = stringBuilder.ToString();

        await fileArtifactGenerationStrategy.GenerateAsync(generator, model);
    }
}