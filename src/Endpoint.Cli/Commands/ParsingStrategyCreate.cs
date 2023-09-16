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

[Verb("parsing-strategy-create")]
public class ParsingStrategyCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('i', "input")]
    public string Input { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ParsingStrategyCreateRequestHandler : IRequestHandler<ParsingStrategyCreateRequest>
{
    private readonly IArtifactGenerator artifactGenerator;
    private readonly ILogger<ParsingStrategyCreateRequestHandler> logger;
    private readonly IFileFactory fileFactory;
    private readonly INamespaceProvider namespaceProvider;

    public ParsingStrategyCreateRequestHandler(
        ILogger<ParsingStrategyCreateRequestHandler> logger,
        IArtifactGenerator artifactGenerator,
        IFileFactory fileFactory,
        INamespaceProvider namespaceProvider)
    {
        this.artifactGenerator = artifactGenerator;
        this.logger = logger;
        this.fileFactory = fileFactory;
        this.namespaceProvider = namespaceProvider;
    }

    public async Task Handle(ParsingStrategyCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating Parsing Strategy: {name}", request.Name);

        var tokens = new TokensBuilder()
            .With("ModelName", request.Name)
            .With("InputName", request.Input)
            .With("Namespace", namespaceProvider.Get(request.Directory))
            .Build();

        var model = fileFactory.CreateTemplate("ParsingStrategy", $"{request.Name}{request.Input}ParsingStrategy", request.Directory, tokens: tokens);

        await artifactGenerator.GenerateAsync(model);
    }
}