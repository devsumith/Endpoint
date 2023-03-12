// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Endpoint.Cli.Commands;


[Verb("controller-method-add")]
public class ControllerMethodAddRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ControllerMethodAddRequestHandler : IRequestHandler<ControllerMethodAddRequest>
{
    private readonly ILogger<ControllerMethodAddRequestHandler> _logger;

    public ControllerMethodAddRequestHandler(ILogger<ControllerMethodAddRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(ControllerMethodAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ControllerMethodAddRequestHandler));
    }
}
