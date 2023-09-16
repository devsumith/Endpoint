// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<ReadmeCreateRequestHandler> logger;
    private readonly IFileFactory fileFactory;
    private readonly IArtifactGenerator artifactGenerator;

    public ReadmeCreateRequestHandler(
        ILogger<ReadmeCreateRequestHandler> logger,
        IFileFactory fileFactory,
        IArtifactGenerator artifactGenerator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(ReadmeCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(ReadmeCreateRequestHandler));

        var model = fileFactory.CreateTemplate("Readme", "README", request.Directory, ".md", tokens: new TokensBuilder()
            .With(nameof(request.ProjectName), request.ProjectName)
            .Build());

        await artifactGenerator.GenerateAsync(model);
    }
}
