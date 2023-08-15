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
    private readonly ILogger<AngularTestRequestHandler> _logger;
    private readonly IAngularService _angularService;
    public AngularTestRequestHandler(
        ILogger<AngularTestRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(AngularTestRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Testing Angular. {searchPattern}", request.SearchPattern);

        await _angularService.Test(request.Directory, request.SearchPattern);
    }
}
