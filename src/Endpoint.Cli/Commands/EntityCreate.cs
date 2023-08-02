// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Services;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Factories;
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
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly IClassModelFactory _classModelFactory;
    public EntityAddRequestHandler(
        ILogger<EntityAddRequestHandler> logger,
        IClassService classService,
        IArtifactGenerator artifactGenerator,
        INamingConventionConverter namingConventionConverter,
        IClassModelFactory classModelFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _classService = classService ?? throw new ArgumentNullException(nameof(classService));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _classModelFactory = classModelFactory ?? throw new ArgumentNullException(nameof(classModelFactory));

    }

    public async Task Handle(EntityCreateRequest request, CancellationToken cancellationToken)
    {
        var model = _classModelFactory.CreateEntity(request.Name, request.Properties);

        await _classService.CreateAsync(request.Name, request.Properties, request.Directory);

        await _classService.CreateAsync($"{request.Name}Dto", request.Properties, request.Directory);

        var dtoExtensions = new DtoExtensionsModel(_namingConventionConverter, $"{model.Name}Extensions", model);

        await _artifactGenerator.CreateAsync(new ObjectFileModel<ClassModel>(dtoExtensions, $"{model.Name}Extensions", request.Directory, "cs"));

    }
}
