// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Syntax.SpecFlow;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("spec-flow-feature-create")]
public class SpecFlowFeatureCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SpecFlowFeatureCreateRequestHandler : IRequestHandler<SpecFlowFeatureCreateRequest>
{
    private readonly ILogger<SpecFlowFeatureCreateRequestHandler> _logger;
    private readonly IArtifactGenerator _artifactGenerator;

    public SpecFlowFeatureCreateRequestHandler(
        ILogger<SpecFlowFeatureCreateRequestHandler> logger,
        IArtifactGenerator artifactGenerator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(SpecFlowFeatureCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(SpecFlowFeatureCreateRequestHandler));

        var model = new SpecFlowFeatureModel(request.Name);

        var fileModel = new ObjectFileModel<SpecFlowFeatureModel>(model, model.Name, request.Directory, "feature");

        await _artifactGenerator.CreateAsync(fileModel);
    }
}
