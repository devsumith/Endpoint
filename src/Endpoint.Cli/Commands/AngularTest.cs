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

[Verb("ng-test")]
public class AngularTestRequest : IRequest
{
    [Option('s', "search-pattern")]
    public string SearchPattern { get; set; } = "*.spec.ts";

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularTestRequestHandler : IRequestHandler<AngularTestRequest>
{
    private readonly ILogger<AngularTestRequestHandler> logger;
    private readonly IAngularService angularService;

    public AngularTestRequestHandler(
        ILogger<AngularTestRequestHandler> logger,
        IAngularService angularService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(AngularTestRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Testing Angular. {searchPattern}", request.SearchPattern);

        await angularService.Test(request.Directory, request.SearchPattern);
    }
}
