// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class TemplatedFileArtifactGenerationStrategy : GenericArtifactGenerationStrategy<TemplatedFileModel>
{
    private readonly ILogger<TemplatedFileArtifactGenerationStrategy> logger;
    private readonly ITemplateProcessor templateProcessor;
    private readonly ISolutionNamespaceProvider solutionNamespaceProvider;
    private readonly ITemplateLocator templateLocator;
    private readonly IGenericArtifactGenerationStrategy<FileModel> fileModelGenerationStrategy;

    public TemplatedFileArtifactGenerationStrategy(
        IGenericArtifactGenerationStrategy<FileModel> fileModelGenerationStrategy,
        ITemplateProcessor templateProcessor,
        ITemplateLocator templateLocator,
        ISolutionNamespaceProvider solutionNamespaceProvider,
        ILogger<TemplatedFileArtifactGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
        this.solutionNamespaceProvider = solutionNamespaceProvider ?? throw new ArgumentNullException(nameof(solutionNamespaceProvider));
        this.templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
        this.fileModelGenerationStrategy = fileModelGenerationStrategy ?? throw new ArgumentNullException(nameof(fileModelGenerationStrategy));
    }

    public override async Task GenerateAsync(IArtifactGenerator generator, TemplatedFileModel model)
    {
        logger.LogInformation("Generating artifact for {0}.", model);

        var template = templateLocator.Get(model.Template);

        var tokens = new TokensBuilder()
            .With("SolutionNamespace", solutionNamespaceProvider.Get(model.Directory))
            .Build();

        foreach (var token in tokens)
        {
            try
            {
                model.Tokens.Add(token.Key, token.Value);
            }
            catch { }
        }

        var result = templateProcessor.Process(template, model.Tokens);

        model.Body = string.Join(Environment.NewLine, result);

        await fileModelGenerationStrategy.GenerateAsync(generator, model);
    }
}
