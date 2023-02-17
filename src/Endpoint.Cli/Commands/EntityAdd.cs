// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Files.Create;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;

[Verb("entity-add")]
public class EntityAddRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('p', "properties")]
    public string Properties { get; set; }

    [Option('d', "directory")]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

public class EntityAddRequestHandler : IRequestHandler<EntityAddRequest>
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

    public async Task Handle(EntityAddRequest request, CancellationToken cancellationToken)
    {
        /*        _logger.LogInformation($"Handled: {nameof(EntityAddRequestHandler)}");

                var model = new EntityFileModelFactory().Create(request.Name, request.Properties, request.Directory, _fileNamespaceProvider.Get(request.Directory));

                _fileGenerationStrategyFactory.CreateFor(model);*/


    }
}
