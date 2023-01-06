using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.WebArtifacts;
using Endpoint.Core.Enums;

namespace Endpoint.Application.Commands;


[Verb("ng-new")]
public class AngularNewRequest : IRequest<Unit> {
    [Option('n',"name")]
    public string Name { get; set; }

    [Option('p')]
    public string Prefix { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

public class AngularNewRequestHandler : IRequestHandler<AngularNewRequest, Unit>
{
    private readonly ILogger<AngularNewRequestHandler> _logger;
    private readonly IWebGenerationStrategyFactory _webGenerationStrategyFactory;
    private readonly IWebArtifactModelsFactory _webArtifactModelsFactory;

    public AngularNewRequestHandler(
        IWebGenerationStrategyFactory webGenerationStrategyFactory,
        IWebArtifactModelsFactory webArtifactModelsFactory,
        ILogger<AngularNewRequestHandler> logger
        )
    {        
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _webGenerationStrategyFactory = webGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(webGenerationStrategyFactory));
        _webArtifactModelsFactory = webArtifactModelsFactory ?? throw new ArgumentNullException(nameof(webArtifactModelsFactory));
    }

    public async Task<Unit> Handle(AngularNewRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handled: {nameof(AngularNewRequestHandler)}");

        var model = _webArtifactModelsFactory.Create(request.Name, request.Prefix, request.Directory);

        _webGenerationStrategyFactory.CreateFor(model);

        return new();
    }
}