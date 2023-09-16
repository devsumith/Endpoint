// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Syntax.SpecFlow;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<SpecFlowFeatureCreateRequestHandler> logger;
    private readonly IArtifactGenerator artifactGenerator;

    public SpecFlowFeatureCreateRequestHandler(
        ILogger<SpecFlowFeatureCreateRequestHandler> logger,
        IArtifactGenerator artifactGenerator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(SpecFlowFeatureCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(SpecFlowFeatureCreateRequestHandler));

        var model = new SpecFlowFeatureModel(request.Name);

        var fileModel = new CodeFileModel<SpecFlowFeatureModel>(model, model.Name, request.Directory, "feature");

        await artifactGenerator.GenerateAsync(fileModel);
    }
}
