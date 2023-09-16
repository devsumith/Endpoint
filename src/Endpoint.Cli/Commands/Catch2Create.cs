using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files.Factories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("catch2-create")]
public class Catch2CreateRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class Catch2CreateRequestHandler : IRequestHandler<Catch2CreateRequest>
{
    private readonly ILogger<Catch2CreateRequestHandler> logger;
    private readonly IFileFactory fileFactory;
    private readonly IArtifactGenerator artifactGenerator;

    public Catch2CreateRequestHandler(
        ILogger<Catch2CreateRequestHandler> logger,
        IFileFactory fileFactory,
        IArtifactGenerator artifactGenerator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(Catch2CreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Generating Catch2", nameof(Catch2CreateRequestHandler));

        var model = fileFactory.CreateTemplate("Catch2", "catch", request.Directory, ".hpp", "catch");

        await artifactGenerator.GenerateAsync(model);
    }
}