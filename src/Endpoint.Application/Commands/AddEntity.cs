using CommandLine;
using Endpoint.Core.Models.Artifacts.Entities;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Files.Create;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Commands;

[Verb("entity-add")]
public class EntityAddRequest : IRequest<Unit>
{
    [Option('n', "name")]
    public string Name { get; set; }
    
    [Option('p', "properties")]
    public string Properties { get; set; }

    [Option('d', "directory")]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

public class EntityAddRequestHandler : IRequestHandler<EntityAddRequest, Unit>
{
    private readonly ILogger _logger;
    private readonly IFileGenerationStrategyFactory _fileGenerationStrategyFactory;
    private readonly IFileNamespaceProvider _fileNamespaceProvider;

    public EntityAddRequestHandler(ILogger logger, IFileGenerationStrategyFactory fileGenerationStrategyFactory, IFileNamespaceProvider fileNamespaceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileGenerationStrategyFactory = fileGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(fileGenerationStrategyFactory));
        _fileNamespaceProvider = fileNamespaceProvider ?? throw new ArgumentNullException(nameof(fileNamespaceProvider));
    }

    public async Task<Unit> Handle(EntityAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handled: {nameof(EntityAddRequestHandler)}");

        var model = EntityFileModelFactory.Create(request.Name, request.Properties, request.Directory, _fileNamespaceProvider.Get(request.Directory));

        _fileGenerationStrategyFactory.CreateFor(model);

        return new();
    }
}