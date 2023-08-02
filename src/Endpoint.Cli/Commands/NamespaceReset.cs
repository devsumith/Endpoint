// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;
using Endpoint.Core.Services;
using System.Collections.Generic;

namespace Endpoint.Cli.Commands;


[Verb("namespace-reset")]
public class NamespaceResetRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class NamespaceResetRequestHandler : IRequestHandler<NamespaceResetRequest>
{
    private readonly ILogger<NamespaceResetRequestHandler> _logger;
    private readonly INamespaceProvider _namespaceProvider;

    public NamespaceResetRequestHandler(
        ILogger<NamespaceResetRequestHandler> logger,
        INamespaceProvider namespaceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
    }

    public async Task Handle(NamespaceResetRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(NamespaceResetRequestHandler));

        foreach (var path in Directory.GetFiles(request.Directory, "*.cs", SearchOption.AllDirectories))
        {
            var @namespace = _namespaceProvider.Get(Path.GetDirectoryName(path));

            var contents = new List<string>();

            bool writeRequired = false;

            var namespaceSyntax = $"namespace {@namespace};";

            foreach (var line in File.ReadLines(path))
            {
                var modified = line;

                if (modified.Trim().StartsWith("namespace") && modified != namespaceSyntax)
                {
                    writeRequired = true;

                    modified = namespaceSyntax;
                }

                contents.Add(modified);
            }

            if (writeRequired)
            {
                File.WriteAllLines(path, contents);
            }
        }
    }
}
