// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Syntax.Namespaces.Factories;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Artifacts;

namespace Endpoint.Cli.Commands;


[Verb("namespace-create")]
public class NamespaceCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class NamespaceCreateRequestHandler : IRequestHandler<NamespaceCreateRequest>
{
    private readonly ILogger<NamespaceCreateRequestHandler> _logger;
    private readonly INamespaceFactory _namespaceFactory;
    private readonly IClassFactory _classFactory;
    private readonly IArtifactGenerator _artifactGenerator;

    public NamespaceCreateRequestHandler(INamespaceFactory namespaceFactory, IClassFactory classFactory, IArtifactGenerator artifactGenerator, ILogger<NamespaceCreateRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _namespaceFactory = namespaceFactory ?? throw new ArgumentNullException(nameof(namespaceFactory));
        _classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(NamespaceCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(NamespaceCreateRequestHandler));
    }
}