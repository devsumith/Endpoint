// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Syntax.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using static Endpoint.Core.Constants.FileExtensions;

namespace Endpoint.Cli.Commands;

[Verb("interface-create")]
public class InterfaceCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class InterfaceCreateRequestHandler : IRequestHandler<InterfaceCreateRequest>
{
    private readonly ILogger<InterfaceCreateRequestHandler> logger;
    private readonly IArtifactGenerator artifactGenerator;

    public InterfaceCreateRequestHandler(
        ILogger<InterfaceCreateRequestHandler> logger,
        IArtifactGenerator artifactGenerator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(InterfaceCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating interface. {name}", request.Name);

        foreach (var name in request.Name.Split(','))
        {
            var model = new InterfaceModel(name);

            model.Methods.Add(new ()
            {
                ParentType = model,
                Name = "Foo",
                Body = new (string.Empty),
            });

            await artifactGenerator.GenerateAsync(new CodeFileModel<InterfaceModel>(model, model.Usings, name, request.Directory, CSharpFile));
        }
    }
}
