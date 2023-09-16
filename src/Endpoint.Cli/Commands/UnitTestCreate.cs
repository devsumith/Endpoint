// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Files.Services;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly IClassService classService;
    private readonly ILogger<UnitTestCreateRequestHandler> logger;

    public UnitTestCreateRequestHandler(IClassService classService, ILogger<UnitTestCreateRequestHandler> logger)
    {
        this.classService = classService ?? throw new ArgumentNullException(nameof(classService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(UnitTestCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation(string.Empty, request.Name);

        await classService.UnitTestCreateAsync(request.Name, request.Methods, request.Directory);
    }
}
