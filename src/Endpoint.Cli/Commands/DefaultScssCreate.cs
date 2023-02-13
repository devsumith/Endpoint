// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.WebArtifacts.Services;

namespace Endpoint.Cli.Commands;


[Verb("default-scss-create")]
public class DefaultScssCreateRequest : IRequest {
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class DefaultScssCreateRequestHandler : IRequestHandler<DefaultScssCreateRequest>
{
    private readonly ILogger<DefaultScssCreateRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public DefaultScssCreateRequestHandler(
        ILogger<DefaultScssCreateRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task<Unit> Handle(DefaultScssCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(DefaultScssCreateRequestHandler));

        _angularService.DefaultScssCreate(request.Directory);

        return new();
    }
}
