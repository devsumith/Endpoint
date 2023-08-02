// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;

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
    private readonly IFileModelFactory _fileModelFactory;
    private readonly IArtifactGenerator _artifactGenerator;

    public ReadmeCreateRequestHandler(
        ILogger<ReadmeCreateRequestHandler> logger,
        IFileModelFactory fileModelFactory,
        IArtifactGenerator artifactGenerator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(ReadmeCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ReadmeCreateRequestHandler));

        var model = _fileModelFactory.CreateTemplate("Readme", "README", request.Directory, "md", tokens: new TokensBuilder()
            .With(nameof(request.ProjectName), request.ProjectName)
            .Build());

        _artifactGenerator.CreateFor(model);

    }
}
