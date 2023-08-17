// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class TemplatedFileArtifactGenerationStrategy : GenericArtifactGenerationStrategy<TemplatedFileModel>
{
    private readonly ILogger<TemplatedFileArtifactGenerationStrategy> _logger;
    private readonly ITemplateProcessor _templateProcessor;
    private readonly ISolutionNamespaceProvider _solutionNamespaceProvider;
    private readonly ITemplateLocator _templateLocator;
    private readonly IGenericArtifactGenerationStrategy<FileModel> _fileModelGenerationStrategy;
    public TemplatedFileArtifactGenerationStrategy(
        IGenericArtifactGenerationStrategy<FileModel> fileModelGenerationStrategy,
        ITemplateProcessor templateProcessor,
        ITemplateLocator templateLocator,
        ISolutionNamespaceProvider solutionNamespaceProvider,
        ILogger<TemplatedFileArtifactGenerationStrategy> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
        _solutionNamespaceProvider = solutionNamespaceProvider ?? throw new ArgumentNullException(nameof(solutionNamespaceProvider));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
        _fileModelGenerationStrategy = fileModelGenerationStrategy ?? throw new ArgumentNullException(nameof(fileModelGenerationStrategy));
    }


    public override async Task GenerateAsync(IArtifactGenerator generator, TemplatedFileModel model, dynamic context = null)
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

        await _fileModelGenerationStrategy.GenerateAsync(generator, model, null);
    }
}
