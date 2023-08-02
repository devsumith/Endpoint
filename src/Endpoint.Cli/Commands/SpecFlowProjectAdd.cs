// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;
using Endpoint.Core.Services;
using Endpoint.Core.Artifacts.Projects.Factories;
using Endpoint.Core.Artifacts.Projects.Services;
using Endpoint.Core.Artifacts.Projects;

namespace Endpoint.Cli.Commands;


[Verb("spec-flow-project-add")]
public class SpecFlowProjectAddRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SpecFlowProjectAddRequestHandler : IRequestHandler<SpecFlowProjectAddRequest>
{
    private readonly ILogger<SpecFlowProjectAddRequestHandler> _logger;
    private readonly IProjectModelFactory _projectModelFactory;
    private readonly IProjectService _projectService;
    private readonly ICommandService _commandService;
    public SpecFlowProjectAddRequestHandler(
        ILogger<SpecFlowProjectAddRequestHandler> logger,
        IProjectModelFactory projectModelFactory,
        IProjectService projectService,
        ICommandService commandService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _projectModelFactory = projectModelFactory ?? throw new ArgumentNullException(nameof(projectModelFactory));
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(SpecFlowProjectAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(SpecFlowProjectAddRequestHandler));

        var done = false;

        var i = 1;

        while (!done)
        {
            if (Directory.Exists($"{request.Directory}{Path.DirectorySeparatorChar}{request.Name}"))
            {
                if (Directory.Exists($"{request.Directory}{Path.DirectorySeparatorChar}{request.Name}_{i}"))
                {
                    i++;
                }
                else
                {
                    request.Name = $"{request.Name}_{i}";

                    SpecFlowProjectAdd();
                }
            }
            else
            {
                SpecFlowProjectAdd();
            }

            void SpecFlowProjectAdd()
            {
                ProjectModel model = _projectModelFactory.CreateSpecFlowProject(request.Name, request.Directory);

                _projectService.AddProject(model);

                done = true;
            }


        }



    }
}
