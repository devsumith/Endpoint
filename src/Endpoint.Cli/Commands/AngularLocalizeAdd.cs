// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.AngularProjects;
using Endpoint.Core.Artifacts.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("ng-localize-add")]
public class AngularLocalizeAddRequest : IRequest
{
    [Option('n', "name", Required = true)]
    public string Name { get; set; }

    [Option('l', "locales")]
    public string Locales { get; set; } = "fr-CA";

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularLocalizeAddRequestHandler : IRequestHandler<AngularLocalizeAddRequest>
{
    private readonly ILogger<AngularLocalizeAddRequestHandler> logger;
    private readonly IAngularService angularService;

    public AngularLocalizeAddRequestHandler(
        ILogger<AngularLocalizeAddRequestHandler> logger,
        IAngularService angularService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(AngularLocalizeAddRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(AngularLocalizeAddRequestHandler));

        await angularService.LocalizeAdd(new AngularProjectReferenceModel(request.Name, request.Directory), request.Locales.Split(',').ToList());
    }
}
