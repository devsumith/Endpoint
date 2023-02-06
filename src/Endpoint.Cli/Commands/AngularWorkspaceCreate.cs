// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Models.WebArtifacts.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("ng-new")]
internal class AngularWorkspaceCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }

    [Option('p', "project-name")]
    public string ProjectName { get; set; } = "app";

    [Option("prefix")]
    public string Prefix { get; set; } = "app";

    [Option('t', "type")]
    public string ProjectType { get; set; } = "application";

    [Option('d', "directory")]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

internal class AngularWorkspaceCreateRequestHandler : IRequestHandler<AngularWorkspaceCreateRequest>
{
    private readonly ILogger<AngularWorkspaceCreateRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public AngularWorkspaceCreateRequestHandler(
        ILogger<AngularWorkspaceCreateRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task<Unit> Handle(AngularWorkspaceCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handled: {nameof(AngularWorkspaceCreateRequestHandler)}");

        _angularService.CreateWorkspace(request.Name, request.ProjectName, request.ProjectType, request.Prefix, request.Directory);

        return new();
    }
}

