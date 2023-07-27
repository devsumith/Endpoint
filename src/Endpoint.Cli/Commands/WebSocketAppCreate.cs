// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.Artifacts.Solutions;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Endpoint.Core.Models.Syntax.Classes.Factories;
using Endpoint.Core.Models.Artifacts.Files.Factories;

namespace Endpoint.Cli.Commands;
[Verb("ws-app-create")]
public class WebSocketAppCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class WebSocketAppCreateRequestHandler : IRequestHandler<WebSocketAppCreateRequest>
{
    private readonly ILogger<WebSocketAppCreateRequestHandler> _logger;
    private readonly ISolutionService _solutionService;
    private readonly ISolutionModelFactory _solutionModelFactory;
    private readonly IArtifactGenerationStrategyFactory _generator;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly ICommandService _commandService;
    private readonly IClassModelFactory _classModelFactory;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;
    private readonly IFileModelFactory _fileModelFactory;

    public WebSocketAppCreateRequestHandler(
        ILogger<WebSocketAppCreateRequestHandler> logger,
        ISolutionService solutionService,
        ISolutionModelFactory solutionModelFactory,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        INamingConventionConverter namingConventionConverter,
        ICommandService commandService,
        IClassModelFactory classModelFactory,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        IFileModelFactory fileModelFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        _solutionModelFactory = solutionModelFactory;
        _generator = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter)); ;
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _classModelFactory = classModelFactory ?? throw new ArgumentNullException(nameof(classModelFactory));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
    }

    public async Task Handle(WebSocketAppCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(WebSocketAppCreateRequestHandler));

        var model = _solutionModelFactory.Create(request.Name, request.Name, "web", string.Empty, request.Directory);


        if(System.IO.Directory.Exists(model.SolutionDirectory))
        {
            _fileSystem.DeleteDirectory(model.SolutionDirectory);
        }

        _generator.CreateFor(model);

        _commandService.Start($"start {model.SolultionFileName}", model.SolutionDirectory);
    }
}
