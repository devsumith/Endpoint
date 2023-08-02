// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

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
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly ILogger<VerbRequestHandler> _logger;
    private readonly IFileModelFactory _fileFactory;
    private readonly INamespaceProvider _namespaceProvider;

    public SyntaxGenerationStrategyCreateRequestHandler(
        ILogger<VerbRequestHandler> logger,
        IArtifactGenerator artifactGenerator,
        IFileModelFactory fileFactory,
        INamespaceProvider namespaceProvider
        )
    {
        _artifactGenerator = artifactGenerator;
        _logger = logger;
        _fileFactory = fileFactory;
        _namespaceProvider = namespaceProvider;
    }

    public async Task Handle(SyntaxGenerationStrategyCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handled: {nameof(SyntaxGenerationStrategyCreateRequestHandler)}");

        var tokens = new TokensBuilder()
            .With("ModelName", request.Name)
            .With("Namespace", _namespaceProvider.Get(request.Directory))
            .Build();

        var model = _fileFactory.CreateTemplate("SyntaxGenerationStrategy", $"{request.Name}SyntaxGenerationStrategy", request.Directory, tokens: tokens);

        await _artifactGenerator.CreateAsync(model);


    }
}
