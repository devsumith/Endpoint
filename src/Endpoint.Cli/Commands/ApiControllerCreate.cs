// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.SystemModels;
using Endpoint.Core.Services;
using Endpoint.Core.Artifacts;

namespace Endpoint.Cli.Commands;


[Verb("api-controller-create")]
public class ApiControllerCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ApiControllerCreateRequestHandler : IRequestHandler<ApiControllerCreateRequest>
{
    private readonly ILogger<ApiControllerCreateRequestHandler> _logger;
    private readonly ISystemContextFactory _systemContextFactory;
    private readonly IContext _context;
    private readonly IArtifactGenerator _artifactGenerator;

    public ApiControllerCreateRequestHandler(ILogger<ApiControllerCreateRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(ApiControllerCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ApiControllerCreateRequestHandler));
    }
}