// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("reference-add")]
public class ReferenceAddRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ReferenceAddRequestHandler : IRequestHandler<ReferenceAddRequest>
{
    private readonly ILogger<ReferenceAddRequestHandler> logger;

    public ReferenceAddRequestHandler(ILogger<ReferenceAddRequestHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(ReferenceAddRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(ReferenceAddRequestHandler));
    }
}
