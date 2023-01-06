using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Solutions;
using Endpoint.Core.Options;
using Endpoint.Core.Strategies.Solutions.Crerate;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Commands;

[Verb("http-project")]
internal class HttpProjectRequest : IRequest<Unit>
{
    [Option('n',"name")]
    public string Name { get; set; }
    [Option('d', "directory")]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

internal class HttpProjectRequestHandler : IRequestHandler<HttpProjectRequest, Unit>
{
    private readonly ILogger _logger;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    public HttpProjectRequestHandler(ILogger logger, IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
    }

    public async Task<Unit> Handle(HttpProjectRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handled: {nameof(HttpProjectRequestHandler)}");

        var model = new SolutionModelFactory().CreateHttpSolution(new CreateEndpointSolutionOptions
        {
            Name = request.Name,
            Directory = request.Directory,
        });

        _artifactGenerationStrategyFactory.CreateFor(model);

        return new();
    }
}
