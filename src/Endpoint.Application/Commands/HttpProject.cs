using CommandLine;
using Endpoint.Core.Factories;
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
    private readonly ISolutionGenerationStrategy _solutionGenerationStrategy;
    public HttpProjectRequestHandler(ILogger logger, ISolutionGenerationStrategy solutionGenerationStrategy)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _solutionGenerationStrategy = solutionGenerationStrategy ?? throw new ArgumentNullException(nameof(solutionGenerationStrategy));
    }

    public async Task<Unit> Handle(HttpProjectRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handled: {nameof(HttpProjectRequestHandler)}");

        var model = SolutionModelFactory.CreateHttpSolution(new CreateEndpointSolutionOptions
        {
            Name = request.Name,
            Directory = request.Directory,
        });

        _solutionGenerationStrategy.Create(model);

        return new();
    }
}
