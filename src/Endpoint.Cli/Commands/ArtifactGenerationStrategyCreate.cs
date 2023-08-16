// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("artifact-generation-strategy-create")]
public class ArtifactGenerationStrategyCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ArtifactGenerationStrategyCreateRequestHandler : IRequestHandler<ArtifactGenerationStrategyCreateRequest>
{
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly ILogger<VerbRequestHandler> _logger;
    private readonly IFileFactory _fileFactory;
    private readonly INamespaceProvider _namespaceProvider;

    public ArtifactGenerationStrategyCreateRequestHandler(
        ILogger<VerbRequestHandler> logger,
        IArtifactGenerator artifactGenerator,
        IFileFactory fileFactory,
        INamespaceProvider namespaceProvider
        )
    {
        _artifactGenerator = artifactGenerator;
        _logger = logger;
        _fileFactory = fileFactory;
        _namespaceProvider = namespaceProvider;
    }

    public async Task Handle(ArtifactGenerationStrategyCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handled: {nameof(ArtifactGenerationStrategyCreateRequestHandler)}");

        var @namespace = _namespaceProvider.Get(request.Directory);

        var tokens = new TokensBuilder()
            .With("ModelName", (SyntaxToken)request.Name)
            .With("Namespace", (SyntaxToken)@namespace)
            .Build();

        var model = _fileFactory.CreateTemplate("ArtifactGenerationStrategy", $"{request.Name}ArtifactGenerationStrategy", request.Directory, tokens: tokens);

        await _artifactGenerator.GenerateAsync(model);


    }
}
