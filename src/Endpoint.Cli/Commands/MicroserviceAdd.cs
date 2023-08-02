// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Projects;
using Endpoint.Core.Artifacts.Projects.Enums;
using Endpoint.Core.Artifacts.Projects.Services;
using Endpoint.Core.Artifacts.Projects;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("microservice-add")]
public class MicroserviceAddRequest : IRequest
{
    [Option('n', "name")]
    public string Names { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class MicroserviceAddRequestHandler : IRequestHandler<MicroserviceAddRequest>
{
    private readonly ILogger<MicroserviceAddRequestHandler> _logger;
    private readonly IProjectService _projectService;
    private readonly IFileSystem _fileSystem;
    private readonly IFileModelFactory _fileModelFactory;

    public MicroserviceAddRequestHandler(
        ILogger<MicroserviceAddRequestHandler> logger,
        IProjectService projectService,
        IFileSystem fileSystem,
        IFileModelFactory fileModelFactory)
    {
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
    }

    public async Task Handle(MicroserviceAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(MicroserviceAddRequestHandler));


        foreach (var name in request.Names.Split(','))
        {
            var microserviceRootDirectory = $"{request.Directory}{Path.DirectorySeparatorChar}{name}";

            _fileSystem.CreateDirectory(microserviceRootDirectory);

            foreach (var layer in new List<string> { "Core", "Infrastructure", "Api" })
            {
                var microserviceDirectory = $"{request.Directory}{Path.DirectorySeparatorChar}{name}{Path.DirectorySeparatorChar}{name}.{layer}";

                var microservice = new ProjectModel { Name = $"{name}.{layer}", Directory = microserviceDirectory, DotNetProjectType = DotNetProjectType.ClassLib };

                if (layer == "Core")
                {
                    microservice.Packages.AddRange(new PackageModel[]
                    {
                        new PackageModel("MediatR", "12.0.0"),
                        new PackageModel("Microsoft.EntityFrameworkCore", "7.0.2"),
                        new PackageModel("Microsoft.Extensions.Logging.Abstractions","7.0.0"),
                        new PackageModel("FluentValidation","11.6.0")
                    });

                    microservice.Files.Add(_fileModelFactory.CreateCoreUsings(microservice.Directory));

                    microservice.Files.Add(_fileModelFactory.CreateResponseBase(microservice.Directory));

                    microservice.Files.Add(_fileModelFactory.CreateLinqExtensions(microservice.Directory));

                }

                if (layer == "Infrastructure")
                {
                    microservice.References.Add(@$"..\{name}.Core\{name}.Core.csproj");
                }

                if (layer == "Api")
                {
                    microservice.References.Add(@$"..\{name}.Infrastructure\{name}.Infrastructure.csproj");
                    microservice.DotNetProjectType = DotNetProjectType.Web;
                }

                _projectService.AddProjectAsync(microservice);

                _projectService.AddProjectAsync(new ProjectModel
                {
                    Name = $"{name}.{layer}.Tests",
                    Directory = $"{request.Directory}{Path.DirectorySeparatorChar}{name}{Path.DirectorySeparatorChar}{name}.{layer}.Tests",
                    DotNetProjectType = DotNetProjectType.XUnit,
                    References = new List<string>() { @$"..\{name}.{layer}\{name}.{layer}.csproj" }
                });
            }
        }


    }
}
