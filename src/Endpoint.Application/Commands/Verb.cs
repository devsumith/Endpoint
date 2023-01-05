using CommandLine;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Commands;

[Verb("verb")]
public class VerbRequest: IRequest<Unit>
{

}

public class VerbRequestHandler : IRequestHandler<VerbRequest, Unit>
{
    private readonly ILogger<VerbRequestHandler> _logger;

    public VerbRequestHandler(ILogger<VerbRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task<Unit> Handle(VerbRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled....");

        return new();
    }
}
