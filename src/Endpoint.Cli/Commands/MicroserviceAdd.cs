// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Projects;
using Endpoint.Core.Artifacts.Projects.Enums;
using Endpoint.Core.Artifacts.Projects.Factories;
using Endpoint.Core.Artifacts.Projects.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("microservice-add")]
public class MicroserviceAddRequest : IRequest
{
    [Option('n', "name")]
    public string Names { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

public class MicroserviceAddRequestHandler : IRequestHandler<MicroserviceAddRequest>
{
    private readonly ILogger<MicroserviceAddRequestHandler> logger;
    private readonly IProjectService projectService;
    private readonly IFileSystem fileSystem;
    private readonly IFileFactory fileFactory;
    private readonly IProjectFactory projectFactory;

    public MicroserviceAddRequestHandler(
        ILogger<MicroserviceAddRequestHandler> logger,
        IProjectFactory projectFactory,
        IProjectService projectService,
        IFileSystem fileSystem,
        IFileFactory fileFactory)
    {
        this.projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        this.projectFactory = projectFactory ?? throw new ArgumentNullException(nameof(projectFactory));
    }

    public async Task Handle(MicroserviceAddRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Adding Microservice", nameof(MicroserviceAddRequestHandler));

        foreach (var name in request.Names.Split(','))
        {
            var microserviceRootDirectory = $"{request.Directory}{Path.DirectorySeparatorChar}{name}";

            fileSystem.Directory.CreateDirectory(microserviceRootDirectory);

            foreach (var layer in new List<string> { "Core", "Infrastructure", "Api" })
            {
                var microserviceDirectory = $"{request.Directory}{Path.DirectorySeparatorChar}{name}{Path.DirectorySeparatorChar}{name}.{layer}";

                var microservice = new ProjectModel { Name = $"{name}.{layer}", Directory = microserviceDirectory, DotNetProjectType = DotNetProjectType.ClassLib };

                if (layer == "Core")
                {
                    microservice.Packages.AddRange(new PackageModel[]
                    {
                        new ("MediatR", "12.0.0"),
                        new ("Microsoft.Extensions.Hosting.Abstractions", "7.0.0"),
                        new ("Microsoft.EntityFrameworkCore", "7.0.2"),
                        new ("Microsoft.Extensions.Logging.Abstractions", "7.0.0"),
                        new ("FluentValidation", "11.6.0"),
                        new ("FluentValidation.DependencyInjectionExtensions", "11.5.1"),
                        new ("Microsoft.EntityFrameworkCore", "7.0.2"),
                        new ("Newtonsoft.Json", "13.0.2"),
                        new ("SerilogTimings", "3.0.1"),
                        new ("System.IdentityModel.Tokens.Jwt", "6.25.1"),
                    });

                    microservice.Files.Add(fileFactory.CreateCoreUsings(microservice.Directory));

                    microservice.Files.Add(fileFactory.CreateResponseBase(microservice.Directory));

                    microservice.Files.Add(fileFactory.CreateLinqExtensions(microservice.Directory));
                }

                if (layer == "Infrastructure")
                {
                    microservice.Packages.AddRange(new PackageModel[]
                    {
                        new ("Microsoft.EntityFrameworkCore.SqlServer", "7.0.5"),
                        new ("Microsoft.EntityFrameworkCore.Design", "12.0.0"),
                        new ("Microsoft.EntityFrameworkCore.Tools", "7.0.5"),
                    });

                    microservice.References.Add(@$"..\{name}.Core\{name}.Core.csproj");
                }

                if (layer == "Api")
                {
                    microservice.Packages.AddRange(new PackageModel[]
                    {
                        new ("Microsoft.AspNetCore.Mvc.Versioning", "5.0.0"),
                        new ("Microsoft.AspNetCore.OpenApi", "7.0.2"),
                        new ("Serilog", "2.12.0"),
                        new ("Serilog.AspNetCore", "6.0.1"),
                        new ("SerilogTimings", "3.0.1"),
                        new ("Swashbuckle.AspNetCore", "6.5.0"),
                        new ("Swashbuckle.AspNetCore.Annotations", "6.5.0"),
                        new ("Swashbuckle.AspNetCore.Swagger", "6.5.0"),
                        new ("Swashbuckle.AspNetCore.Newtonsoft", "6.5.0"),
                    });

                    microservice.References.Add(@$"..\{name}.Infrastructure\{name}.Infrastructure.csproj");
                    microservice.DotNetProjectType = DotNetProjectType.Web;
                }

                await projectService.AddProjectAsync(microservice);

                await projectService.AddProjectAsync(new ProjectModel
                {
                    Name = $"{name}.{layer}.Tests",
                    Directory = $"{request.Directory}{Path.DirectorySeparatorChar}{name}{Path.DirectorySeparatorChar}{name}.{layer}.Tests",
                    DotNetProjectType = DotNetProjectType.XUnit,
                    References = new List<string>() { @$"..\{name}.{layer}\{name}.{layer}.csproj" },
                });
            }
        }
    }
}
