// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.AngularProjects;
using Endpoint.Core.Artifacts.Services;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<AngularConfigurationCreateRequestHandler> logger;
    private readonly IAngularService angularService;

    public AngularConfigurationCreateRequestHandler(
        ILogger<AngularConfigurationCreateRequestHandler> logger,
        IAngularService angularService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(AngularConfigurationCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(AngularConfigurationCreateRequestHandler));

        await angularService.AddBuildConfiguration(request.Name, new AngularProjectReferenceModel(request.Project, request.Directory));
    }
}
