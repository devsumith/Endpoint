// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("ts-model-add")]
public class TypeScriptModelAddRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('p', "properties")]
    public string Properties { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class TypeScriptModelAddRequestHandler : IRequestHandler<TypeScriptModelAddRequest>
{
    private readonly ILogger<TypeScriptModelAddRequestHandler> logger;
    private readonly IAngularService angularService;

    public TypeScriptModelAddRequestHandler(
        ILogger<TypeScriptModelAddRequestHandler> logger,
        IAngularService angularService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(TypeScriptModelAddRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(TypeScriptModelAddRequestHandler));

        await angularService.ModelCreate(request.Name, request.Directory, request.Properties);
    }
}
