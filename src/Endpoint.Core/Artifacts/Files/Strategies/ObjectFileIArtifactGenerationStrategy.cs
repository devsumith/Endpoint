// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Internals;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public abstract class ObjectFileIArtifactGenerationStrategy<T> : FileGenerationStrategy, IArtifactGenerationStrategy<ObjectFileModel<T>>
{
    private readonly ILogger<ObjectFileIArtifactGenerationStrategy<T>> _logger;
    private readonly ISyntaxGenerator _syntaxGenerator;
    private readonly IFileSystem _fileSystem;
    private readonly INamespaceProvider _namespaceProvider;
    private readonly Observable<INotification> _notificationListener;

    public ObjectFileIArtifactGenerationStrategy(
        ISyntaxGenerator syntaxGenerator,
        IFileSystem fileSystem,
        INamespaceProvider namespaceProvider,
        Observable<INotification> notificationListener,
        ILogger<ObjectFileIArtifactGenerationStrategy<T>> logger)
        :base(default, fileSystem)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _syntaxGenerator = syntaxGenerator ?? throw new ArgumentNullException(nameof(syntaxGenerator));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
        _notificationListener = notificationListener ?? throw new ArgumentNullException(nameof(notificationListener));
    }

    public async Task GenerateAsync(IArtifactGenerator artifactGenerator, ObjectFileModel<T> model, dynamic context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        var builder = new StringBuilder();

        foreach (var @using in model.Usings)
        {
            builder.AppendLine($"using {@using.Name};");

            if (@using == model.Usings.Last())
            {
                builder.AppendLine();
            }
        }

        var fileNamespace = string.IsNullOrEmpty(model.Namespace) ? _namespaceProvider.Get(model.Directory) : model.Namespace;

        if (!string.IsNullOrEmpty(fileNamespace) && fileNamespace != "NamespaceNotFound" && !fileNamespace.Contains(".lib."))
        {
            builder.AppendLine($"namespace {fileNamespace};");

            builder.AppendLine();
        }

        builder.AppendLine(await _syntaxGenerator.GenerateAsync(model.Object, context));

        model.Content = builder.ToString();

        await base.GenerateAsync(artifactGenerator, model, null);
    }
}
