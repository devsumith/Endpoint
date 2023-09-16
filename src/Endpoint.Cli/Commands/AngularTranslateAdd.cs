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

[Verb("ng-translate-add")]
public class AngularTranslateAddRequest : IRequest
{
    [Option('n', "name")]
    public string ProjectName { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularTranslateAddRequestHandler : IRequestHandler<AngularTranslateAddRequest>
{
    private readonly ILogger<AngularTranslateAddRequestHandler> logger;
    private readonly IAngularService angularService;

    public AngularTranslateAddRequestHandler(
        IAngularService angularService,
        ILogger<AngularTranslateAddRequestHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(AngularTranslateAddRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(AngularTranslateAddRequestHandler));

        await angularService.NgxTranslateAdd(request.ProjectName, request.Directory);
    }
}
