// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Models.Artifacts.Files.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;

[Verb("unit-test-create")]
public class UnitTestCreateRequest : IRequest
{
    [Option('n')]
    public string Name { get; set; }

    [Option('m')]
    public string Methods { get; set; }

    [Option('d')]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class UnitTestCreateRequestHandler : IRequestHandler<UnitTestCreateRequest>
{
    private readonly IClassService _classService;
    private readonly ILogger<UnitTestCreateRequestHandler> _logger;

    public UnitTestCreateRequestHandler(IClassService classService, ILogger<UnitTestCreateRequestHandler> logger)
    {
        _classService = classService ?? throw new ArgumentNullException(nameof(classService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(UnitTestCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("", request.Name);

        _classService.UnitTestCreateFor(request.Name, request.Methods, request.Directory);

        return new ();
    }
}

