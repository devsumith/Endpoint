// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts.Projects;
using Endpoint.Core.Artifacts.Projects.Factories;
using Endpoint.Core.Artifacts.Projects.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
    private readonly IProjectFactory _projectFactory;
    private readonly IProjectService _projectService;
    private readonly ICommandService _commandService;
    public PlaywrightProjectAddRequestHandler(
        ILogger<PlaywrightProjectAddRequestHandler> logger,
        IProjectFactory projectFactory,
        IProjectService projectService,
        ICommandService commandService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _projectFactory = projectFactory ?? throw new ArgumentNullException(nameof(projectFactory));
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

            ProjectModel model = await _projectFactory.CreatePlaywrightProject(request.Name, request.Directory);


            _projectService.AddProjectAsync(model);

            _commandService.Start("dotnet build", model.Directory);

            _commandService.Start($"powershell {model.Directory}{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}Debug{Path.DirectorySeparatorChar}net7.0{Path.DirectorySeparatorChar}playwright.ps1 install", model.Directory);

            done = true;
        }



    }
}
