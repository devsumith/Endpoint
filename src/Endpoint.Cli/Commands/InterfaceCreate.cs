// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.Syntax.Interfaces;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;

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
    private readonly ILogger<InterfaceCreateRequestHandler> _logger;
    private readonly IArtifactGenerator _artifactGenerator;

    public InterfaceCreateRequestHandler(
        ILogger<InterfaceCreateRequestHandler> logger,
        IArtifactGenerator artifactGenerator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(InterfaceCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(InterfaceCreateRequestHandler));

        foreach (var name in request.Name.Split(','))
        {
            var model = new InterfaceModel(name);

            _artifactGenerator.CreateFor(new ObjectFileModel<InterfaceModel>(model, model.UsingDirectives, name, request.Directory, "cs"));
        }

    }
}
