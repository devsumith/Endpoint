// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.WebArtifacts.Services;
using Endpoint.Core.Services;
using System.IO;
using Endpoint.Core.Models.WebArtifacts;

namespace Endpoint.Cli.Commands;


[Verb("ng-project-add")]
public class AngularAddProjectRequest : IRequest {
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
    private readonly ILogger<AngularAddProjectRequestHandler> _logger;
    private readonly IAngularService _angularService;
    private readonly IFileProvider _fileProvider;

    public AngularAddProjectRequestHandler(
        ILogger<AngularAddProjectRequestHandler> logger,
        IAngularService angularService,
        IFileProvider fileProvider
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
    }

    public async Task Handle(AngularAddProjectRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AngularAddProjectRequestHandler));

        var workspaceDirectory = Path.GetDirectoryName(_fileProvider.Get("angular.json", request.Directory));

        _angularService.AddProject(new AngularProjectModel(request.Name, request.ProjectType, request.Prefix, workspaceDirectory));
    }
}