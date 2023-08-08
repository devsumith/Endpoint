// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class TemplatedFileArtifactGenerationStrategy : FileGenerationStrategy, IArtifactGenerationStrategy<TemplatedFileModel>
{
    private readonly ILogger<TemplatedFileArtifactGenerationStrategy> _logger;
    private readonly ITemplateProcessor _templateProcessor;
    private readonly ISolutionNamespaceProvider _solutionNamespaceProvider;

    public TemplatedFileArtifactGenerationStrategy(
        ITemplateProcessor templateProcessor,
        ITemplateLocator templateLocator,
        IFileSystem fileSystem,
        ISolutionNamespaceProvider solutionNamespaceProvider,
        ILogger<TemplatedFileArtifactGenerationStrategy> logger,
        ILoggerFactory loggerFactory)
        :base(loggerFactory.CreateLogger<FileGenerationStrategy>(), fileSystem, templateLocator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
        _solutionNamespaceProvider = solutionNamespaceProvider ?? throw new ArgumentNullException(nameof(solutionNamespaceProvider));
    }

    public async Task GenerateAsync(IArtifactGenerator artifactGenerator, TemplatedFileModel model, dynamic? context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        var template = _templateLocator.Get(model.Template);

        var tokens = new TokensBuilder()
            .With("SolutionNamespace", _solutionNamespaceProvider.Get(model.Directory))
            .Build();

        foreach (var token in tokens)
        {
            model.Tokens.Add(token.Key, token.Value);
        }

        var result = _templateProcessor.Process(template, model.Tokens);

        model.Content = string.Join(Environment.NewLine, result);

        await base.GenerateAsync(artifactGenerator, model as FileModel, null);

    }
}
