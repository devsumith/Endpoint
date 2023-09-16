// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("verb")]
public class VerbRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class VerbRequestHandler : IRequestHandler<VerbRequest>
{
    private readonly IArtifactGenerator artifactGenerator;
    private readonly ILogger<VerbRequestHandler> logger;
    private readonly IFileFactory fileFactory;
    private readonly INamespaceProvider namespaceProvider;

    public VerbRequestHandler(
        ILogger<VerbRequestHandler> logger,
        IArtifactGenerator artifactGenerator,
        IFileFactory fileFactory,
        INamespaceProvider namespaceProvider)
    {
        this.artifactGenerator = artifactGenerator;
        this.logger = logger;
        this.fileFactory = fileFactory;
        this.namespaceProvider = namespaceProvider;
    }

    public async Task Handle(VerbRequest request, CancellationToken cancellationToken)
    {
        var @namespace = namespaceProvider.Get(request.Directory);

        var tokens = new TokensBuilder()
            .With("Name", (SyntaxToken)request.Name)
            .With("Namespace", (SyntaxToken)@namespace)
            .Build();

        var model = fileFactory.CreateTemplate("Verb", request.Name, request.Directory, tokens: tokens);

        await artifactGenerator.GenerateAsync(model);
    }
}
