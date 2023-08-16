// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Solutions;
using Endpoint.Core.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

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
    private readonly ILogger _logger;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly ISolutionFactory _solutionFactory;
    public HttpProjectRequestHandler(ILogger logger, IArtifactGenerator artifactGenerator, ISolutionFactory solutionFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _solutionFactory = solutionFactory ?? throw new ArgumentNullException(nameof(solutionFactory));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(HttpProjectRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handled: {nameof(HttpProjectRequestHandler)}");

        var model = await _solutionFactory.CreateHttpSolution(new CreateEndpointSolutionOptions
        {
            Name = request.Name,
            Directory = request.Directory,
        });

        await _artifactGenerator.GenerateAsync(model);


    }
}

