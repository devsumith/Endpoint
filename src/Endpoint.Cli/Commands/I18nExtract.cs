// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts.AngularProjects;
using Endpoint.Core.Artifacts.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("i18n-extract")]
public class I18nExtractRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class I18nExtractRequestHandler : IRequestHandler<I18nExtractRequest>
{
    private readonly ILogger<I18nExtractRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public I18nExtractRequestHandler(
        ILogger<I18nExtractRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(I18nExtractRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(I18nExtractRequestHandler));

        await _angularService.I18nExtract(new AngularProjectReferenceModel(request.Name, request.Directory));

    }
}
