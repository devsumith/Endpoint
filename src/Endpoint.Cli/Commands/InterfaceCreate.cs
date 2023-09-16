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
        logger.LogInformation("Handled: {0}", nameof(InterfaceCreateRequestHandler));

        foreach (var name in request.Name.Split(','))
        {
            var model = new InterfaceModel(name);

            await artifactGenerator.GenerateAsync(new CodeFileModel<InterfaceModel>(model, model.Usings, name, request.Directory, ".cs"));
        }
    }
}
