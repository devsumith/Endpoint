// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Solutions;
using Endpoint.Core.Syntax.Classes.Factories;

namespace Endpoint.Cli.Commands;
[Verb("ws-app-create")]
public class WebSocketAppCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class WebSocketAppCreateRequestHandler : IRequestHandler<WebSocketAppCreateRequest>
{
    private readonly ILogger<WebSocketAppCreateRequestHandler> _logger;
    private readonly ISolutionService _solutionService;
    private readonly ISolutionFactory _solutionFactory;
    private readonly IArtifactGenerator _generator;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly ICommandService _commandService;
    private readonly IClassFactory _classFactory;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;
    private readonly IFileFactory _fileFactory;

    public WebSocketAppCreateRequestHandler(
        ILogger<WebSocketAppCreateRequestHandler> logger,
        ISolutionService solutionService,
        ISolutionFactory solutionFactory,
        IArtifactGenerator artifactGenerator,
        INamingConventionConverter namingConventionConverter,
        ICommandService commandService,
        IClassFactory classFactory,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        IFileFactory fileFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        _solutionFactory = solutionFactory;
        _generator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter)); ;
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
    }

    public async Task Handle(WebSocketAppCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(WebSocketAppCreateRequestHandler));

        var model = await _solutionFactory.Create(request.Name, request.Name, "web", string.Empty, request.Directory);


        if (System.IO.Directory.Exists(model.SolutionDirectory))
        {
            _fileSystem.DeleteDirectory(model.SolutionDirectory);
        }

        await _generator.GenerateAsync(model);

        _commandService.Start($"start {model.SolultionFileName}", model.SolutionDirectory);
    }
}
