using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts;

namespace Endpoint.Cli.Commands;


[Verb("catch2-create")]
public class Catch2CreateRequest : IRequest {
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class Catch2CreateRequestHandler : IRequestHandler<Catch2CreateRequest>
{
    private readonly ILogger<Catch2CreateRequestHandler> _logger;
    private readonly IFileFactory _fileFactory;
    private readonly IArtifactGenerator _artifactGenerator;
    public Catch2CreateRequestHandler(
        ILogger<Catch2CreateRequestHandler> logger,
        IFileFactory fileFactory,
        IArtifactGenerator artifactGenerator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(Catch2CreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Generating Catch2", nameof(Catch2CreateRequestHandler));

        var model = _fileFactory.CreateTemplate("Catch2", "catch", request.Directory, ".hpp", "catch");

        await _artifactGenerator.GenerateAsync(model);
    }
}