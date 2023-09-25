// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("get-full-path")]
public class GetFullPathRequest : IRequest
{
    [Option('r', "relative")]
    public string RelativePath { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class GetFullPathRequestHandler : IRequestHandler<GetFullPathRequest>
{
    private readonly ILogger<GetFullPathRequestHandler> logger;

    public GetFullPathRequestHandler(ILogger<GetFullPathRequestHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(GetFullPathRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Resolving Relative Path. {relativePath}", request.RelativePath);

        Console.WriteLine(Path.GetFullPath(request.RelativePath, request.Directory));
    }
}