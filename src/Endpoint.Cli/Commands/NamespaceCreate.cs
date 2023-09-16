// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Namespaces.Factories;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<NamespaceCreateRequestHandler> logger;
    private readonly INamespaceFactory namespaceFactory;
    private readonly IClassFactory classFactory;
    private readonly IArtifactGenerator artifactGenerator;

    public NamespaceCreateRequestHandler(INamespaceFactory namespaceFactory, IClassFactory classFactory, IArtifactGenerator artifactGenerator, ILogger<NamespaceCreateRequestHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.namespaceFactory = namespaceFactory ?? throw new ArgumentNullException(nameof(namespaceFactory));
        this.classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(NamespaceCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(NamespaceCreateRequestHandler));
    }
}