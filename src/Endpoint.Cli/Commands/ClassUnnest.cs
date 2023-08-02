// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("class-unnest")]
public class ClassUnnestRequest : IRequest
{

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ClassUnnestRequestHandler : IRequestHandler<ClassUnnestRequest>
{
    private readonly ILogger<ClassUnnestRequestHandler> _logger;

    public ClassUnnestRequestHandler(ILogger<ClassUnnestRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(ClassUnnestRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(NamespaceUnnestRequestHandler));

        await AddSuffix(request, cancellationToken).ConfigureAwait(false);

        await Unnest(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task Unnest(ClassUnnestRequest request, CancellationToken cancellationToken)
    {
        foreach (var path in Directory.GetFiles(request.Directory, "*.cs", SearchOption.AllDirectories))
        {
            if (nestedClasses(path))
            {
                Console.WriteLine(path);

                var modifiedContent = new List<string>();

                foreach (var line in File.ReadLines(path))
                {
                    var classOpenOrCloseBracket = line.StartsWith("{") || line.StartsWith("}") || line.StartsWith(" {") || line.StartsWith(" }");

                    if (!classOpenOrCloseBracket && !line.StartsWith("public class") && !line.StartsWith(" public class"))
                    {
                        if (line.StartsWith("    "))
                        {
                            modifiedContent.Add(line.Substring(3));
                        }
                        else
                        {
                            modifiedContent.Add(line);
                        }
                    }
                }

                File.WriteAllLines(path, modifiedContent);
            }
        }

        bool nestedClasses(string path)
        {
            var nestedClasses = false;

            var classContext = false;

            foreach (var line in File.ReadLines(path))
            {
                if (classContext && line.Contains("public class") && line.StartsWith("    "))
                {
                    nestedClasses = true;
                }

                if (line.StartsWith("public class") || line.StartsWith(" public class"))
                {
                    classContext = true;
                }

            }

            return nestedClasses;
        }
    }

    public async Task AddSuffix(ClassUnnestRequest request, CancellationToken cancellationToken)
    {
        foreach (var path in Directory.GetFiles(request.Directory, "*.cs", SearchOption.AllDirectories))
        {
            var suffix = Path.GetFileNameWithoutExtension(path);

            var content = new List<string>();

            foreach (var line in File.ReadLines(path))
            {
                var modified = line;

                modified = modified.Replace("public class Request :", $"public class {suffix}Request :");

                modified = modified.Replace("IRequest<Response>", $"IRequest<{suffix}Response>");

                modified = modified.Replace("public class Response", $"public class {suffix}Response");

                modified = modified.Replace("public class Handler", $"public class {suffix}Handler");

                modified = modified.Replace("IRequestHandler<Request, Response>", $"IRequestHandler<{suffix}Request, {suffix}Response>");

                modified = modified.Replace("public Handler", $"public {suffix}Handler");

                modified = modified.Replace("Task<Response>", $"Task<{suffix}Response>");

                modified = modified.Replace("(Request request,", $"({suffix}Request request,");

                modified = modified.Replace("new Response", $"new {suffix}Response");

                modified = modified.Replace("new Response", $"new {suffix}Response");

                modified = modified.Replace("<Request>", $"<{suffix}Request>");

                modified = modified.Replace("<Response>", $"<{suffix}Response>");

                modified = modified.Replace("class Validator", $"class {suffix}Validator");

                modified = modified.Replace("public Validator", $"public {suffix}Validator");

                content.Add(modified);
            }

            File.WriteAllLines(path, content);
        }
    }
}
