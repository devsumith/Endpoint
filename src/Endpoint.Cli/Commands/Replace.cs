// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("replace")]
public class ReplaceRequest : IRequest
{
    [Option('s', "search")]
    public string Old { get; set; }

    [Option('r', "replace")]
    public string New { get; set; }

    [Option('p')]
    public string Pattern { get; set; } = "*.*";

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ReplaceRequestHandler : IRequestHandler<ReplaceRequest>
{
    private readonly ILogger<ReplaceRequestHandler> logger;

    public ReplaceRequestHandler(ILogger<ReplaceRequestHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(ReplaceRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(ReplaceRequestHandler));

        foreach (var path in Directory.GetFiles(request.Directory, request.Pattern, SearchOption.AllDirectories))
        {
            var modified = new List<string>();

            var containsOldValue = false;

            foreach (var line in File.ReadLines(path))
            {
                if (line.Contains(request.Old))
                {
                    containsOldValue = true;
                }

                modified.Add(line.Replace(request.Old, request.New));
            }

            if (containsOldValue)
            {
                File.WriteAllLines(path, modified);
            }
        }
    }
}
