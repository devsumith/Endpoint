// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files.Factories;

namespace Endpoint.Cli.Commands;


[Verb("editor-config-create")]
public class EditorConfigCreateRequest : IRequest
{

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class EditorConfigCreateRequestHandler : IRequestHandler<EditorConfigCreateRequest>
{
    private readonly ILogger<EditorConfigCreateRequestHandler> _logger;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly IFileFactory _fileModelFactory;

    public EditorConfigCreateRequestHandler(
        ILogger<EditorConfigCreateRequestHandler> logger,
        IArtifactGenerator artifactGenerator,
        IFileFactory fileModelFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
    }

    public async Task Handle(EditorConfigCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(EditorConfigCreateRequestHandler));

        var model = _fileModelFactory.CreateTemplate("EditorConfig", string.Empty, request.Directory, "editorconfig", string.Empty);

        await _artifactGenerator.CreateAsync(model);
    }
}
