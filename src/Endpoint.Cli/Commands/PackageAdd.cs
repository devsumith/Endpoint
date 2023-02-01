// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Services;
using Endpoint.Core.Models.Artifacts.Projects.Services;

namespace Endpoint.Cli.Commands;


[Verb("package-add")]
public class PackageAddRequest : IRequest<Unit> {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class PackageAddRequestHandler : IRequestHandler<PackageAddRequest, Unit>
{
    private readonly ILogger<PackageAddRequestHandler> _logger;
    private readonly IProjectService _projectService;

    public PackageAddRequestHandler(ILogger<PackageAddRequestHandler> logger, IProjectService projectService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
    }

    public async Task<Unit> Handle(PackageAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(PackageAddRequestHandler));

        _projectService.PackageAdd(request.Name, request.Directory);

        return new();
    }
}
