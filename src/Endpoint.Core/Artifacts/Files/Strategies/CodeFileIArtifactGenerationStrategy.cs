// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Text;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public abstract class CodeFileIArtifactGenerationStrategy<T> : GenericArtifactGenerationStrategy<CodeFileModel<T>>
    where T : SyntaxModel
{
    protected readonly ILogger<CodeFileIArtifactGenerationStrategy<T>> logger;
    protected readonly ISyntaxGenerator syntaxGenerator;
    protected readonly IFileSystem fileSystem;
    protected readonly INamespaceProvider namespaceProvider;
    protected readonly IGenericArtifactGenerationStrategy<FileModel> fileArtifactGenerationStrategy;

    public CodeFileIArtifactGenerationStrategy(
        ISyntaxGenerator syntaxGenerator,
        IFileSystem fileSystem,
        INamespaceProvider namespaceProvider,
        IGenericArtifactGenerationStrategy<FileModel> fileArtifactGenerationStrategy,
        ILogger<CodeFileIArtifactGenerationStrategy<T>> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.syntaxGenerator = syntaxGenerator ?? throw new ArgumentNullException(nameof(syntaxGenerator));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
        this.fileArtifactGenerationStrategy = fileArtifactGenerationStrategy ?? throw new ArgumentNullException(nameof(fileArtifactGenerationStrategy));
    }

    public override async Task GenerateAsync(IArtifactGenerator generator, CodeFileModel<T> model)
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

        var fileNamespace = string.IsNullOrEmpty(model.Namespace) ? namespaceProvider.Get(model.Directory) : model.Namespace;

        if (!string.IsNullOrEmpty(fileNamespace) && fileNamespace != "NamespaceNotFound" && !fileNamespace.Contains(".lib."))
        {
            stringBuilder.AppendLine($"namespace {fileNamespace};");

            stringBuilder.AppendLine();
        }

        stringBuilder.AppendLine(await syntaxGenerator.GenerateAsync(model.Object));

        model.Body = stringBuilder.ToString();

        await fileArtifactGenerationStrategy.GenerateAsync(generator, model);
    }
}
