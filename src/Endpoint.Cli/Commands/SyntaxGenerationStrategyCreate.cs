// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("syntax-generation-strategy-create")]
public class SyntaxGenerationStrategyCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SyntaxGenerationStrategyCreateRequestHandler : IRequestHandler<SyntaxGenerationStrategyCreateRequest>
{
    private readonly IArtifactGenerator artifactGenerator;
    private readonly ILogger<SyntaxGenerationStrategyCreateRequestHandler> logger;
    private readonly IFileFactory fileFactory;
    private readonly INamespaceProvider namespaceProvider;

    public SyntaxGenerationStrategyCreateRequestHandler(
        ILogger<SyntaxGenerationStrategyCreateRequestHandler> logger,
        IArtifactGenerator artifactGenerator,
        IFileFactory fileFactory,
        INamespaceProvider namespaceProvider)
    {
        this.artifactGenerator = artifactGenerator;
        this.logger = logger;
        this.fileFactory = fileFactory;
        this.namespaceProvider = namespaceProvider;
    }

    public async Task Handle(SyntaxGenerationStrategyCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handled: {nameof(SyntaxGenerationStrategyCreateRequestHandler)}");

        var tokens = new TokensBuilder()
            .With("ModelName", request.Name)
            .With("Namespace", namespaceProvider.Get(request.Directory))
            .Build();

        var model = fileFactory.CreateTemplate("SyntaxGenerationStrategy", $"{request.Name}SyntaxGenerationStrategy", request.Directory, tokens: tokens);

        await artifactGenerator.GenerateAsync(model);
    }
}
