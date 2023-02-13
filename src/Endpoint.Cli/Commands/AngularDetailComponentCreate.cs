// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Endpoint.Cli.Commands;


[Verb("angular-detail-component-create")]
public class AngularDetailComponentCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularDetailComponentCreateRequestHandler : IRequestHandler<AngularDetailComponentCreateRequest>
{
    private readonly ILogger<AngularDetailComponentCreateRequestHandler> _logger;

    public AngularDetailComponentCreateRequestHandler(ILogger<AngularDetailComponentCreateRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(AngularDetailComponentCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AngularDetailComponentCreateRequestHandler));

        return new();
    }
}
