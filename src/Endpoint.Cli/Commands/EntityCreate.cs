// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Services;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Factories;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<EntityAddRequestHandler> logger;
    private readonly IClassService classService;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly INamingConventionConverter namingConventionConverter;
    private readonly IClassFactory classFactory;

    public EntityAddRequestHandler(
        ILogger<EntityAddRequestHandler> logger,
        IClassService classService,
        IArtifactGenerator artifactGenerator,
        INamingConventionConverter namingConventionConverter,
        IClassFactory classFactory)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.classService = classService ?? throw new ArgumentNullException(nameof(classService));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        this.classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
    }

    public async Task Handle(EntityCreateRequest request, CancellationToken cancellationToken)
    {
        var model = await classFactory.CreateEntityAsync(request.Name, request.Properties);

        await classService.CreateAsync(request.Name, request.Properties, request.Directory);

        await classService.CreateAsync($"{request.Name}Dto", request.Properties, request.Directory);

        // var dtoExtensions = new DtoExtensionsModel(_namingConventionConverter, $"{model.Name}Extensions", model);

        // await _artifactGenerator.GenerateAsync(new ObjectFileModel<ClassModel>(dtoExtensions, $"{model.Name}Extensions", request.Directory, ".cs"));
    }
}
