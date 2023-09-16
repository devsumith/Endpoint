// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files.Factories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("editor-config-create")]
public class EditorConfigCreateRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class EditorConfigCreateRequestHandler : IRequestHandler<EditorConfigCreateRequest>
{
    private readonly ILogger<EditorConfigCreateRequestHandler> logger;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly IFileFactory fileFactory;

    public EditorConfigCreateRequestHandler(
        ILogger<EditorConfigCreateRequestHandler> logger,
        IArtifactGenerator artifactGenerator,
        IFileFactory fileFactory)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
    }

    public async Task Handle(EditorConfigCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(EditorConfigCreateRequestHandler));

        var model = fileFactory.CreateTemplate("EditorConfig", string.Empty, request.Directory, "editorconfig", string.Empty);

        await artifactGenerator.GenerateAsync(model);
    }
}
