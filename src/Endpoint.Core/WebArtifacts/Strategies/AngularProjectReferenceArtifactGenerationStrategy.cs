// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.WebArtifacts.Commands;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.WebArtifacts.Strategies;

public class AddAngularTranslateGenerationStrategy : ArtifactGenerationStrategyBase<AngularProjectReferenceModel>
{
    private readonly ILogger<AddAngularTranslateGenerationStrategy> _logger;
    public AddAngularTranslateGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<AddAngularTranslateGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override bool CanHandle(object model, dynamic context = null)
        => model is AngularProjectReferenceModel && context is AngularTranslateAdd;

    public override void Create(IArtifactGenerator artifactGenerator, AngularProjectReferenceModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

    }
}
