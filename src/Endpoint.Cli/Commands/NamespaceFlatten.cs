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


[Verb("namespace-flatten")]
public class NamespaceFlattenRequest : IRequest {

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class NamespaceFlattenRequestHandler : IRequestHandler<NamespaceFlattenRequest>
{
    private readonly ILogger<NamespaceFlattenRequestHandler> _logger;

    public NamespaceFlattenRequestHandler(ILogger<NamespaceFlattenRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(NamespaceFlattenRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(NamespaceFlattenRequestHandler));

        foreach(var path in Directory.GetFiles(request.Directory,"*.cs",SearchOption.AllDirectories))
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

                    if (line.StartsWith("    "))
                        newContent.Add(line.Substring(3));
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
