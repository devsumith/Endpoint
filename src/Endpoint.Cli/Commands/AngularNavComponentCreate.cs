// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("angular-nav-component-create")]
public class AngularNavComponentCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularNavComponentCreateRequestHandler : IRequestHandler<AngularNavComponentCreateRequest>
{
    private readonly ILogger<AngularNavComponentCreateRequestHandler> logger;

    public AngularNavComponentCreateRequestHandler(ILogger<AngularNavComponentCreateRequestHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(AngularNavComponentCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(AngularNavComponentCreateRequestHandler));
    }
}
