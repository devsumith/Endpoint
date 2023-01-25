using CommandLine;
using Endpoint.Core.Models.WebArtifacts.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("prettier-add")]
public class PrettierAddRequest : IRequest<Unit> {
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class PrettierAddRequestHandler : IRequestHandler<PrettierAddRequest, Unit>
{
    private readonly ILogger<PrettierAddRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public PrettierAddRequestHandler(
        ILogger<PrettierAddRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task<Unit> Handle(PrettierAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(PrettierAddRequestHandler));

        _angularService.PrettierAdd(request.Directory);

        return new();
    }
}