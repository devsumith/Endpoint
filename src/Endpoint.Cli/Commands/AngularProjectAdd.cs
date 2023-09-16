// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.AngularProjects;
using Endpoint.Core.Artifacts.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("ng-project-add")]
public class AngularAddProjectRequest : IRequest
{
    [Option('n', "name", Required = true)]
    public string Name { get; set; }

    [Option('p', "prefix")]
    public string Prefix { get; set; } = "lib";

    [Option('t', "project-type")]
    public string ProjectType { get; set; } = "library";

    [Option('d', "directory", Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularAddProjectRequestHandler : IRequestHandler<AngularAddProjectRequest>
{
    private readonly ILogger<AngularAddProjectRequestHandler> logger;
    private readonly IAngularService angularService;
    private readonly IFileProvider fileProvider;

    public AngularAddProjectRequestHandler(
        ILogger<AngularAddProjectRequestHandler> logger,
        IAngularService angularService,
        IFileProvider fileProvider)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
    }

    public async Task Handle(AngularAddProjectRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(AngularAddProjectRequestHandler));

        var workspaceDirectory = Path.GetDirectoryName(fileProvider.Get("angular.json", request.Directory));

        await angularService.AddProject(new AngularProjectModel(request.Name, request.ProjectType, request.Prefix, workspaceDirectory));
    }
}