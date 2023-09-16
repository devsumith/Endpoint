// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Namespaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("class-from-plant-uml-create")]
public class ClassFromPlantUmlCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ClassFromPlantUmlCreateRequestHandler : IRequestHandler<ClassFromPlantUmlCreateRequest>
{
    private readonly ILogger<ClassFromPlantUmlCreateRequestHandler> logger;
    private readonly IArtifactParser artifactParser;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly IContext context;

    public ClassFromPlantUmlCreateRequestHandler(
        IContext context,
        IArtifactParser artifactParser,
        IArtifactGenerator artifactGenerator,
        ILogger<ClassFromPlantUmlCreateRequestHandler> logger)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.artifactParser = artifactParser ?? throw new ArgumentNullException(nameof(artifactParser));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(ClassFromPlantUmlCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating Class from PlantUml");

        context.Set(new CodeFileModel<NamespaceModel>()
        {
            Directory = request.Directory,
            Name = request.Name,
        });

        var model = await artifactParser.ParseAsync<CodeFileModel<NamespaceModel>>("Foo");

        await artifactGenerator.GenerateAsync(model);
    }
}