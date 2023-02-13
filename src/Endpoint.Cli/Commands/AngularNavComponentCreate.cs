// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Endpoint.Cli.Commands;


[Verb("angular-nav-component-create")]
public class AngularNavComponentCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularNavComponentCreateRequestHandler : IRequestHandler<AngularNavComponentCreateRequest>
{
    private readonly ILogger<AngularNavComponentCreateRequestHandler> _logger;

    public AngularNavComponentCreateRequestHandler(ILogger<AngularNavComponentCreateRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(AngularNavComponentCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AngularNavComponentCreateRequestHandler));

        return new();
    }
}
