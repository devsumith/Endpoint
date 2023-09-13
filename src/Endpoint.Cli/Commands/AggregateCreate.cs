// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Folders.Factories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

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
    private readonly IArtifactGenerator _artifactGenerator;

    public AggregateCreateRequestHandler(
        ILogger<AggregateCreateRequestHandler> logger,
        IFolderFactory folderFactory,
        IArtifactGenerator artifactGenerator
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _folderFactory = folderFactory ?? throw new ArgumentNullException(nameof(folderFactory));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(AggregateCreateRequest request, CancellationToken cancellationToken)
    {

        _logger.LogInformation("Creating Aggregate. {name}", request.Name);

        var model = await _folderFactory.CreateAggregateAsync(request.Name, request.Properties, request.Directory);

        await _artifactGenerator.GenerateAsync(model);

    }
}
