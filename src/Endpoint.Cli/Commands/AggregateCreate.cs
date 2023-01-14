using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Endpoint.Cli.Commands;


[Verb("aggregate-create")]
public class AggregateCreateRequest : IRequest<Unit> {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AggregateCreateRequestHandler : IRequestHandler<AggregateCreateRequest, Unit>
{
    private readonly ILogger<AggregateCreateRequestHandler> _logger;

    public AggregateCreateRequestHandler(ILogger<AggregateCreateRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(AggregateCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AggregateCreateRequestHandler));

        return new();
    }
}