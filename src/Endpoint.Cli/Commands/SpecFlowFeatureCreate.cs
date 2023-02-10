// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Syntax.SpecFlow;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax;
using System.Collections.Generic;

namespace Endpoint.Cli.Commands;


[Verb("spec-flow-feature-create")]
public class SpecFlowFeatureCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SpecFlowFeatureCreateRequestHandler : IRequestHandler<SpecFlowFeatureCreateRequest>
{
    private readonly ILogger<SpecFlowFeatureCreateRequestHandler> _logger;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;

    public SpecFlowFeatureCreateRequestHandler(
        ILogger<SpecFlowFeatureCreateRequestHandler> logger,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
    }

    public async Task<Unit> Handle(SpecFlowFeatureCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(SpecFlowFeatureCreateRequestHandler));

        var model = new SpecFlowFeatureModel(request.Name);

        var fileModel = new ObjectFileModel<SpecFlowFeatureModel>(model, model.Name, request.Directory, "feature");

        _artifactGenerationStrategyFactory.CreateFor(fileModel);

        return new();
    }
}
