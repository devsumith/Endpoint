// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Projects;
using Endpoint.Core.Artifacts.Projects.Factories;
using Endpoint.Core.Artifacts.Projects.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<PlaywrightProjectAddRequestHandler> logger;
    private readonly IProjectFactory projectFactory;
    private readonly IProjectService projectService;
    private readonly ICommandService commandService;

    public PlaywrightProjectAddRequestHandler(
        ILogger<PlaywrightProjectAddRequestHandler> logger,
        IProjectFactory projectFactory,
        IProjectService projectService,
        ICommandService commandService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.projectFactory = projectFactory ?? throw new ArgumentNullException(nameof(projectFactory));
        this.projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(PlaywrightProjectAddRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(PlaywrightProjectAddRequestHandler));

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

            ProjectModel model = await projectFactory.CreatePlaywrightProject(request.Name, request.Directory);

            projectService.AddProjectAsync(model);

            commandService.Start("dotnet build", model.Directory);

            commandService.Start($"powershell {model.Directory}{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}Debug{Path.DirectorySeparatorChar}net7.0{Path.DirectorySeparatorChar}playwright.ps1 install", model.Directory);

            done = true;
        }
    }
}
