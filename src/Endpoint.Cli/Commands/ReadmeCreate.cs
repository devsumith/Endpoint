// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Services;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts;

namespace Endpoint.Cli.Commands;


[Verb("readme-create")]
public class ReadmeCreateRequest : IRequest
{
    [Option('n')]
    public string ProjectName { get; set; } = "Project";

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ReadmeCreateRequestHandler : IRequestHandler<ReadmeCreateRequest>
{
    private readonly ILogger<ReadmeCreateRequestHandler> _logger;
    private readonly IFileFactory _fileFactory;
    private readonly IArtifactGenerator _artifactGenerator;

    public ReadmeCreateRequestHandler(
        ILogger<ReadmeCreateRequestHandler> logger,
        IFileFactory fileFactory,
        IArtifactGenerator artifactGenerator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(ReadmeCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ReadmeCreateRequestHandler));

        var model = _fileFactory.CreateTemplate("Readme", "README", request.Directory, ".md", tokens: new TokensBuilder()
            .With(nameof(request.ProjectName), request.ProjectName)
            .Build());

        await _artifactGenerator.GenerateAsync(model);

    }
}
