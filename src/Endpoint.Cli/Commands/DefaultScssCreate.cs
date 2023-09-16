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

[Verb("default-scss-create")]
public class DefaultScssCreateRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class DefaultScssCreateRequestHandler : IRequestHandler<DefaultScssCreateRequest>
{
    private readonly ILogger<DefaultScssCreateRequestHandler> logger;
    private readonly IAngularService angularService;

    public DefaultScssCreateRequestHandler(
        ILogger<DefaultScssCreateRequestHandler> logger,
        IAngularService angularService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(DefaultScssCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(DefaultScssCreateRequestHandler));

        await angularService.DefaultScssCreate(request.Directory);
    }
}
