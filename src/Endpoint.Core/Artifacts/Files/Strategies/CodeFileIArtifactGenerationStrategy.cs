// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public abstract class CodeFileIArtifactGenerationStrategy<T> : GenericArtifactGenerationStrategy<CodeFileModel<T>>
    where T : SyntaxModel
{
    private readonly ILogger<CodeFileIArtifactGenerationStrategy<T>> _logger;
    private readonly ISyntaxGenerator _syntaxGenerator;
    private readonly IFileSystem _fileSystem;
    private readonly INamespaceProvider _namespaceProvider;
    private readonly IGenericArtifactGenerationStrategy<FileModel> _fileArtifactGenerationStrategy;

    public CodeFileIArtifactGenerationStrategy(
        ISyntaxGenerator syntaxGenerator,
        IFileSystem fileSystem,
        INamespaceProvider namespaceProvider,
        IGenericArtifactGenerationStrategy<FileModel> fileArtifactGenerationStrategy,
        ILogger<CodeFileIArtifactGenerationStrategy<T>> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _syntaxGenerator = syntaxGenerator ?? throw new ArgumentNullException(nameof(syntaxGenerator));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
        _fileArtifactGenerationStrategy = fileArtifactGenerationStrategy ?? throw new ArgumentNullException(nameof(fileArtifactGenerationStrategy));
    }

    public override async Task GenerateAsync(IArtifactGenerator generator, CodeFileModel<T> model, dynamic context = null)
    {
        _logger.LogInformation("Generating Object File. {name}", model.Name);

        var stringBuilder = new StringBuilder();

        foreach (var @using in model.Usings)
        {
            stringBuilder.AppendLine($"using {@using.Name};");

            if (@using == model.Usings.Last())
            {
                stringBuilder.AppendLine();
            }
        }

        var fileNamespace = string.IsNullOrEmpty(model.Namespace) ? _namespaceProvider.Get(model.Directory) : model.Namespace;

        if (!string.IsNullOrEmpty(fileNamespace) && fileNamespace != "NamespaceNotFound" && !fileNamespace.Contains(".lib."))
        {
            stringBuilder.AppendLine($"namespace {fileNamespace};");

            stringBuilder.AppendLine();
        }

        stringBuilder.AppendLine(await _syntaxGenerator.GenerateAsync(model.Object, context));

        model.Body = stringBuilder.ToString();

        await _fileArtifactGenerationStrategy.GenerateAsync(generator, model);
    }
}
