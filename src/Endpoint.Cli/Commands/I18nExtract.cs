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
    private readonly ILogger<I18nExtractRequestHandler> logger;
    private readonly IAngularService angularService;

    public I18nExtractRequestHandler(
        ILogger<I18nExtractRequestHandler> logger,
        IAngularService angularService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(I18nExtractRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(I18nExtractRequestHandler));

        await angularService.I18nExtract(new AngularProjectReferenceModel(request.Name, request.Directory));
    }
}
