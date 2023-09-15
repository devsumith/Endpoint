// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Namespaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("class-from-plant-uml-create")]
public class ClassFromPlantUmlCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ClassFromPlantUmlCreateRequestHandler : IRequestHandler<ClassFromPlantUmlCreateRequest>
{
    private readonly ILogger<ClassFromPlantUmlCreateRequestHandler> _logger;
    private readonly IArtifactParser _artifactParser;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly IContext _context;

    public ClassFromPlantUmlCreateRequestHandler(
        IContext context,
        IArtifactParser artifactParser,
        IArtifactGenerator artifactGenerator,
        ILogger<ClassFromPlantUmlCreateRequestHandler> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _artifactParser = artifactParser ?? throw new ArgumentNullException(nameof(artifactParser));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(ClassFromPlantUmlCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating Class from PlantUml", nameof(ClassFromPlantUmlCreateRequestHandler));

        _context.Set(new CodeFileModel<NamespaceModel>()
        {
            Directory = request.Directory,
            Name = request.Name
        });

        var model = await _artifactParser.ParseAsync<CodeFileModel<NamespaceModel>>("Foo");

        await _artifactGenerator.GenerateAsync(model);
    }
}