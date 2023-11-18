// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Artifacts.Solutions.Factories;
using Endpoint.Core.Services;
using Endpoint.Core.SystemModels;
using Endpoint.Core.Artifacts;

namespace Endpoint.Cli.Commands;


[Verb("ddd-app-slim-create")]
public class DddAppSlimCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }

    [Option('a', "aggregate")]
    public string Aggregate { get; set; }

    [Option('a', "properties")]
    public string Properties { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class DddAppSlimCreateRequestHandler : IRequestHandler<DddAppSlimCreateRequest>
{
    private readonly ILogger<DddAppSlimCreateRequestHandler> _logger;
    private readonly ISolutionFactory _solutionFactory;
    private readonly IContext _context;
    private readonly ISystemContextFactory _systemContextFactory;
    private readonly IArtifactGenerator _artifactGenerator;

    public DddAppSlimCreateRequestHandler(ILogger<DddAppSlimCreateRequestHandler> logger, ISolutionFactory solutionFactory, IContext context, ISystemContextFactory systemContextFactory, IArtifactGenerator artifactGenerator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _solutionFactory = solutionFactory ?? throw new ArgumentNullException(nameof(solutionFactory));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _systemContextFactory = systemContextFactory ?? throw new ArgumentNullException(nameof(systemContextFactory));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(DddAppSlimCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(DddAppSlimCreateRequestHandler));

        var systemContext = await _systemContextFactory.DddSlimCreateAsync(request.Name, request.Aggregate, request.Properties, request.Directory);

        _context.Set(systemContext);
    }
}