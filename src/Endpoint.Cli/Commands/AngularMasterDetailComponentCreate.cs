// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("angular-master-detail-component-create")]
public class AngularMasterDetailComponentCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularMasterDetailComponentCreateRequestHandler : IRequestHandler<AngularMasterDetailComponentCreateRequest>
{
    private readonly ILogger<AngularMasterDetailComponentCreateRequestHandler> logger;

    public AngularMasterDetailComponentCreateRequestHandler(ILogger<AngularMasterDetailComponentCreateRequestHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(AngularMasterDetailComponentCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(AngularMasterDetailComponentCreateRequestHandler));
    }
}
