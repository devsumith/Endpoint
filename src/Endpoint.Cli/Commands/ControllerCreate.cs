// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Models.Artifacts.Projects.Services;
using Endpoint.Core.Models.Syntax.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("controller-create")]
public class ControllerCreateRequest : IRequest
{
    [Option('e')]
    public string EntityName { get; set; }

    [Option('d')]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ControllerCreateRequestHandler : IRequestHandler<ControllerCreateRequest>
{
    private readonly ILogger<ControllerCreateRequestHandler> _logger;
    private readonly IApiProjectService _apiProjectService;

    public ControllerCreateRequestHandler(ILogger<ControllerCreateRequestHandler> logger, IApiProjectService apiProjectService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _apiProjectService = apiProjectService ?? throw new ArgumentNullException(nameof(apiProjectService));
    }

    public async Task<Unit> Handle(ControllerCreateRequest request, CancellationToken cancellationToken)
    {
        var entity = new EntityModel(request.EntityName);

        _apiProjectService.ControllerAdd(entity, request.Directory);

        return new();
    }
}

