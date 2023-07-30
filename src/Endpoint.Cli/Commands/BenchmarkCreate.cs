// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Endpoint.Cli.Commands;


[Verb("benchmark-create")]
public class BenchmarkCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class BenchmarkCreateRequestHandler : IRequestHandler<BenchmarkCreateRequest>
{
    private readonly ILogger<BenchmarkCreateRequestHandler> _logger;

    public BenchmarkCreateRequestHandler(ILogger<BenchmarkCreateRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(BenchmarkCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(BenchmarkCreateRequestHandler));
    }
}
