// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Endpoint.Cli.Commands;


[Verb("angular-domain-component-create")]
public class AngularDomainComponentCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularDomainComponentCreateRequestHandler : IRequestHandler<AngularDomainComponentCreateRequest>
{
    private readonly ILogger<AngularDomainComponentCreateRequestHandler> _logger;

    public AngularDomainComponentCreateRequestHandler(ILogger<AngularDomainComponentCreateRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(AngularDomainComponentCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AngularDomainComponentCreateRequestHandler));


    }
}
