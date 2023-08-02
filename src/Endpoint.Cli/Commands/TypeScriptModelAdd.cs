// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.WebArtifacts.Services;

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
    private readonly ILogger<TypeScriptModelAddRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public TypeScriptModelAddRequestHandler(
        ILogger<TypeScriptModelAddRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(TypeScriptModelAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(TypeScriptModelAddRequestHandler));

        _angularService.ModelCreate(request.Name, request.Directory, request.Properties);


    }
}
