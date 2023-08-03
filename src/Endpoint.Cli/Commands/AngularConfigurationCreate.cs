// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Artifacts.AngularProjects;
using Endpoint.Core.Artifacts.Services;

namespace Endpoint.Cli.Commands;


[Verb("angular-configuration-create")]
public class AngularConfigurationCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('p', "project")]
    public string Project { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularConfigurationCreateRequestHandler : IRequestHandler<AngularConfigurationCreateRequest>
{
    private readonly ILogger<AngularConfigurationCreateRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public AngularConfigurationCreateRequestHandler(
        ILogger<AngularConfigurationCreateRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(AngularConfigurationCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AngularConfigurationCreateRequestHandler));

        await _angularService.AddBuildConfiguration(request.Name, new AngularProjectReferenceModel(request.Project, request.Directory));
    }
}
