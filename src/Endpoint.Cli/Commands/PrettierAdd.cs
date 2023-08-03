// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("prettier-add")]
public class PrettierAddRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class PrettierAddRequestHandler : IRequestHandler<PrettierAddRequest>
{
    private readonly ILogger<PrettierAddRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public PrettierAddRequestHandler(
        ILogger<PrettierAddRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(PrettierAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(PrettierAddRequestHandler));

        await _angularService.PrettierAdd(request.Directory);


    }
}
