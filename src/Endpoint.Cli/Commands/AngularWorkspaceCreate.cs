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

[Verb("ng-new")]
internal class AngularWorkspaceCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('p', "project-name")]
    public string ProjectName { get; set; } = "app";

    [Option("prefix")]
    public string Prefix { get; set; } = "app";

    [Option('t', "type")]
    public string ProjectType { get; set; } = "application";

    [Option('f', "force")]
    public bool Force { get; set; }

    [Option('v', "version")]
    public string Version { get; set; } = "latest";

    [Option('d', "directory")]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

internal class AngularWorkspaceCreateRequestHandler : IRequestHandler<AngularWorkspaceCreateRequest>
{
    private readonly ILogger<AngularWorkspaceCreateRequestHandler> logger;
    private readonly IAngularService angularService;

    public AngularWorkspaceCreateRequestHandler(
        ILogger<AngularWorkspaceCreateRequestHandler> logger,
        IAngularService angularService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(AngularWorkspaceCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handled: {nameof(AngularWorkspaceCreateRequestHandler)}");

        await angularService.CreateWorkspace(request.Name, request.Version, request.ProjectName, request.ProjectType, request.Prefix, request.Directory);
    }
}