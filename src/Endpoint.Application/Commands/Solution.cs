using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Endpoint.Application.Commands;


[Verb("solution")]
public class SolutionRequest : IRequest<Unit> {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SolutionRequestHandler : IRequestHandler<SolutionRequest, Unit>
{
    private readonly ILogger<SolutionRequestHandler> _logger;

    public SolutionRequestHandler(ILogger<SolutionRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(SolutionRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handled: {nameof(SolutionRequestHandler)}");

        

        return new();
    }
}