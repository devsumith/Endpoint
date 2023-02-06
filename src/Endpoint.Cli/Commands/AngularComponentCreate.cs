using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.WebArtifacts.Services;

namespace Endpoint.Cli.Commands;


[Verb("ng-component-create")]
public class AngularComponentCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularComponentCreateRequestHandler : IRequestHandler<AngularComponentCreateRequest>
{
    private readonly ILogger<AngularComponentCreateRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public AngularComponentCreateRequestHandler(
        ILogger<AngularComponentCreateRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task<Unit> Handle(AngularComponentCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AngularComponentCreateRequestHandler));

        _angularService.ComponentCreate(request.Name, request.Directory);

        return new();
    }
}