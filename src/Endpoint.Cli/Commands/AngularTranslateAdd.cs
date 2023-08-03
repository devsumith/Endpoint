// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Artifacts.Services;

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
    private readonly ILogger<AngularTranslateAddRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public AngularTranslateAddRequestHandler(
        IAngularService angularService,
        ILogger<AngularTranslateAddRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(AngularTranslateAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AngularTranslateAddRequestHandler));

        await _angularService.NgxTranslateAdd(request.ProjectName, request.Directory);


    }
}
