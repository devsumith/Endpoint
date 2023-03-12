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


[Verb("cqrs-flatten")]
public class CqrsFlattenRequest : IRequest {

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class CqrsFlattenRequestHandler : IRequestHandler<CqrsFlattenRequest>
{
    private readonly ILogger<CqrsFlattenRequestHandler> _logger;

    public CqrsFlattenRequestHandler(ILogger<CqrsFlattenRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(CqrsFlattenRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(NamespaceFlattenRequestHandler));

        foreach(var path in Directory.GetFiles(request.Directory,"*.cs",SearchOption.AllDirectories))
        {
            var commandName = Path.GetFileNameWithoutExtension(path);

            var newContent = new List<string>();

            var lines = File.ReadLines(path);


            foreach (var line in lines)
            {
                if (line.StartsWith("public class") || line.StartsWith("{") || line.StartsWith("}"))
                {
                    break;
                }                    
                else if(line.Trim() == "")
                {
                    newContent.Add("");
                }
                else if (line.StartsWith("    "))
                {
                    var newLine = line.Substring(4)
                        .Replace(" Validator", $" {commandName}Validator")
                        .Replace("Response", $"{commandName}Response")
                        .Replace("<Request", $"<{commandName}Request")
                        .Replace("(Request", $"({commandName}Request")
                        .Replace("public class Request", $"public class {commandName}Request")
                        .Replace("public Handler", $" public {commandName}RequestHandler")
                        .Replace("public class Handler", $" public class {commandName}RequestHandler");

                    newContent.Add(newLine);
                }
                else
                {
                    newContent.Add(line);
                }

            }

            foreach (var line in newContent)
            {
                Console.WriteLine(line);
            }

            File.WriteAllLines(path, newContent);
        }



    }
}
