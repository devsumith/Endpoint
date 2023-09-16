// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Solutions.Factories;
using Endpoint.Core.Options;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("http-project")]
internal class HttpProjectRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', "directory")]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

internal class HttpProjectRequestHandler : IRequestHandler<HttpProjectRequest>
{
    private readonly ILogger logger;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly ISolutionFactory solutionFactory;

    public HttpProjectRequestHandler(ILogger logger, IArtifactGenerator artifactGenerator, ISolutionFactory solutionFactory)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.solutionFactory = solutionFactory ?? throw new ArgumentNullException(nameof(solutionFactory));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(HttpProjectRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handled: {nameof(HttpProjectRequestHandler)}");

        var model = await solutionFactory.CreateHttpSolution(new CreateEndpointSolutionOptions
        {
            Name = request.Name,
            Directory = request.Directory,
        });

        await artifactGenerator.GenerateAsync(model);
    }
}
