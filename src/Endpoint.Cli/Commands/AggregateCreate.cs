// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.Syntax.Entities.Aggregate;
using Endpoint.Core.Models.Artifacts.Folders.Factories;
using AutoMapper.Execution;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Syntax.Classes;
using System.Dynamic;

namespace Endpoint.Cli.Commands;


[Verb("aggregate-create")]
public class AggregateCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('p', "properties")]
    public string Properties { get; set; }


    [Option('m', "microservice-name")]
    public string MicroserviceName { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AggregateCreateRequestHandler : IRequestHandler<AggregateCreateRequest>
{
    private readonly ILogger<AggregateCreateRequestHandler> _logger;
    private readonly IFolderFactory _folderFactory;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;

    public AggregateCreateRequestHandler(
        ILogger<AggregateCreateRequestHandler> logger,
        IFolderFactory folderFactory,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _folderFactory = folderFactory ?? throw new ArgumentNullException(nameof(folderFactory));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
    }

    public async Task Handle(AggregateCreateRequest request, CancellationToken cancellationToken)
    {

        // Usings

        // Update Db Context

        _logger.LogInformation("Handled: {0}", nameof(AggregateCreateRequestHandler));

        var model = _folderFactory.Aggregate(request.Name, request.Properties, request.Directory);

        _artifactGenerationStrategyFactory.CreateFor(model, new AggregateCreateContext(model.Aggregate));
    }
}

public class AggregateCreateContext
{
    public AggregateCreateContext(ClassModel entity)
    {
        Entity = entity;
    }
    public ClassModel Entity { get; set; }
}
