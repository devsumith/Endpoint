// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Endpoint.Cli.Commands;


[Verb("namespace-unnest")]
public class NamespaceUnnestRequest : IRequest
{

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class NamespaceUnnestRequestHandler : IRequestHandler<NamespaceUnnestRequest>
{
    private readonly ILogger<NamespaceUnnestRequestHandler> _logger;

    public NamespaceUnnestRequestHandler(ILogger<NamespaceUnnestRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(NamespaceUnnestRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(NamespaceUnnestRequestHandler));

        foreach (var path in Directory.GetFiles(request.Directory, "*.*", SearchOption.AllDirectories))
        {
            if (Path.GetExtension(path) == ".cs" || Path.GetExtension(path) == ".txt")
            {
                var newContent = new List<string>();

                var lines = File.ReadLines(path);

                if (lines.FirstOrDefault(x => x.StartsWith("namespace") && !x.Contains(";")) != null)
                {
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("namespace"))
                        {
                            newContent.Add("");
                            newContent.Add($"{line};");
                            newContent.Add("");
                        }

                        if (line.StartsWith("using"))
                        {
                            newContent.Add(line);
                        }

                        if (string.IsNullOrEmpty(line.Trim()))
                        {
                            newContent.Add("");
                        }
                        else if (line.StartsWith("    "))
                            newContent.Add(line.Substring(4));
                    }

                    foreach (var line in newContent)
                    {
                        Console.WriteLine(line);
                    }

                    File.WriteAllLines(path, newContent);
                }
            }

        }


    }
}
