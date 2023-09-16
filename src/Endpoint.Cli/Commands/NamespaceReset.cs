// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("namespace-reset")]
public class NamespaceResetRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class NamespaceResetRequestHandler : IRequestHandler<NamespaceResetRequest>
{
    private readonly ILogger<NamespaceResetRequestHandler> logger;
    private readonly INamespaceProvider namespaceProvider;

    public NamespaceResetRequestHandler(
        ILogger<NamespaceResetRequestHandler> logger,
        INamespaceProvider namespaceProvider)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
    }

    public async Task Handle(NamespaceResetRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(NamespaceResetRequestHandler));

        foreach (var path in Directory.GetFiles(request.Directory, "*.cs", SearchOption.AllDirectories))
        {
            var @namespace = namespaceProvider.Get(Path.GetDirectoryName(path));

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
