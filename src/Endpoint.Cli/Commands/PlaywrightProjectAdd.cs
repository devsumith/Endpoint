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
using System.IO;
using SimpleNLG.Extensions;
using Endpoint.Core.Artifacts.Projects.Factories;
using Endpoint.Core.Artifacts.Projects.Services;
using Endpoint.Core.Artifacts.Projects;

namespace Endpoint.Cli.Commands;


[Verb("playwright-project-add")]
public class PlaywrightProjectAddRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class PlaywrightProjectAddRequestHandler : IRequestHandler<PlaywrightProjectAddRequest>
{
    private readonly ILogger<PlaywrightProjectAddRequestHandler> _logger;
    private readonly IProjectModelFactory _projectModelFactory;
    private readonly IProjectService _projectService;
    private readonly ICommandService _commandService;
    public PlaywrightProjectAddRequestHandler(
        ILogger<PlaywrightProjectAddRequestHandler> logger,
        IProjectModelFactory projectModelFactory,
        IProjectService projectService,
        ICommandService commandService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _projectModelFactory = projectModelFactory ?? throw new ArgumentNullException(nameof(projectModelFactory));
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(PlaywrightProjectAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(PlaywrightProjectAddRequestHandler));

        var done = false;

        var i = 1;

        while (!done)
        {
            if (Directory.Exists($"{request.Directory}{Path.DirectorySeparatorChar}{request.Name}"))
            {
                if (Directory.Exists($"{request.Directory}{Path.DirectorySeparatorChar}{request.Name}_{i}"))
                {
                    i++;
                    break;
                }

                request.Name = $"{request.Name}_{i}";
            }

            ProjectModel model = _projectModelFactory.CreatePlaywrightProject(request.Name, request.Directory);


            _projectService.AddProjectAsync(model);

            _commandService.Start("dotnet build", model.Directory);

            _commandService.Start($"powershell {model.Directory}{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}Debug{Path.DirectorySeparatorChar}net7.0{Path.DirectorySeparatorChar}playwright.ps1 install", model.Directory);

            done = true;
        }



    }
}
