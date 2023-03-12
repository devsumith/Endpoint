// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Files.Services;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Classes.Factories;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;

[Verb("entity-create")]
public class EntityCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('p', "properties")]
    public string Properties { get; set; }

    [Option('d', "directory")]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

public class EntityAddRequestHandler : IRequestHandler<EntityCreateRequest>
{
    private readonly ILogger<EntityAddRequestHandler> _logger;
    private readonly IClassService _classService;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly IClassModelFactory _classModelFactory;
    public EntityAddRequestHandler(
        ILogger<EntityAddRequestHandler> logger, 
        IClassService classService,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        INamingConventionConverter namingConventionConverter,
        IClassModelFactory classModelFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _classService = classService ?? throw new ArgumentNullException(nameof(classService));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _classModelFactory = classModelFactory ?? throw new ArgumentNullException(nameof(classModelFactory));

    }

    public async Task Handle(EntityCreateRequest request, CancellationToken cancellationToken)
    {
        var model = _classModelFactory.CreateEntity(request.Name, request.Properties);

        _classService.Create(request.Name, request.Properties, request.Directory);

        _classService.Create($"{request.Name}Dto", request.Properties, request.Directory);

        var dtoExtensions = new DtoExtensionsModel(_namingConventionConverter, $"{model.Name}Extensions", model);

        _artifactGenerationStrategyFactory.CreateFor(new ObjectFileModel<ClassModel>(dtoExtensions, $"{model.Name}Extensions", request.Directory, "cs"));

    }
}
