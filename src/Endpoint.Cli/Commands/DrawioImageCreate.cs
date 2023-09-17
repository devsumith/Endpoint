// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("drawio-image-create")]
public class DrawioImageCreateRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class DrawioImageCreateRequestHandler : IRequestHandler<DrawioImageCreateRequest>
{
    private readonly ILogger<DrawioImageCreateRequestHandler> logger;
    private readonly ICodeAnalysisService codeAnalysisService;
    private readonly ICommandService commandService;

    public DrawioImageCreateRequestHandler(ILogger<DrawioImageCreateRequestHandler> logger, ICodeAnalysisService codeAnalysisService, ICommandService commandService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.codeAnalysisService = codeAnalysisService ?? throw new ArgumentNullException(nameof(codeAnalysisService));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(DrawioImageCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating image from drawio file. {directory}", request.Directory);

        if (!await codeAnalysisService.IsNpmPackageInstalledAsync("draw.io"))
        {
            commandService.Start("npm install -g draw.io", request.Directory);
        }
    }
}