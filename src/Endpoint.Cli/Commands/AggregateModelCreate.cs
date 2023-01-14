using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Endpoint.Cli.Commands;


[Verb("aggregate-model-create")]
public class AggregateModelCreateRequest : IRequest<Unit> {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AggregateModelCreateRequestHandler : IRequestHandler<AggregateModelCreateRequest, Unit>
{
    private readonly ILogger<AggregateModelCreateRequestHandler> _logger;

    public AggregateModelCreateRequestHandler(ILogger<AggregateModelCreateRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(AggregateModelCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AggregateModelCreateRequestHandler));

        return new();
    }
}