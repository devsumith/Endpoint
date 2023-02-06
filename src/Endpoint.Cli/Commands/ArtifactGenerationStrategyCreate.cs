// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("artifact-generation-strategy-create")]
public class ArtifactGenerationStrategyCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ArtifactGenerationStrategyCreateRequestHandler : IRequestHandler<ArtifactGenerationStrategyCreateRequest>
{
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly ILogger<VerbRequestHandler> _logger;
    private readonly IFileModelFactory _fileFactory;
    private readonly INamespaceProvider _namespaceProvider;

    public ArtifactGenerationStrategyCreateRequestHandler(
        ILogger<VerbRequestHandler> logger,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        IFileModelFactory fileFactory,
        INamespaceProvider namespaceProvider
        )
    {
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory;
        _logger = logger;
        _fileFactory = fileFactory;
        _namespaceProvider = namespaceProvider;
    }

    public async Task<Unit> Handle(ArtifactGenerationStrategyCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handled: {nameof(ArtifactGenerationStrategyCreateRequestHandler)}");

        var @namespace = _namespaceProvider.Get(request.Directory);

        var tokens = new TokensBuilder()
            .With("ModelName", (SyntaxToken)request.Name)
            .With("Namespace", (SyntaxToken)@namespace)
            .Build();

        var model = _fileFactory.CreateTemplate("ArtifactGenerationStrategy", $"{request.Name}ArtifactGenerationStrategy", request.Directory, tokens: tokens);

        _artifactGenerationStrategyFactory.CreateFor(model);

        return new();
    }
}
