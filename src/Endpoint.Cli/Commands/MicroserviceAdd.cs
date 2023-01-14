using CommandLine;
using Endpoint.Core.Models.Artifacts.Projects;
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
public class MicroserviceAddRequest : IRequest<Unit> {
    [Option('n',"name")]
    public string Names { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class MicroserviceAddRequestHandler : IRequestHandler<MicroserviceAddRequest, Unit>
{
    private readonly ILogger<MicroserviceAddRequestHandler> _logger;
    private readonly IProjectService _projectService;
    private readonly IFileSystem _fileSystem;

    public MicroserviceAddRequestHandler(
        ILogger<MicroserviceAddRequestHandler> logger,
        IProjectService projectService,
        IFileSystem fileSystem)
    {
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public async Task<Unit> Handle(MicroserviceAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(MicroserviceAddRequestHandler));


        foreach(var name in request.Names.Split(','))
        {
            var microserviceRootDirectory = $"{request.Directory}{Path.DirectorySeparatorChar}{name}";

            _fileSystem.CreateDirectory(microserviceRootDirectory);

            foreach(var layer in new List<string> { "Core", "Infrastructure", "Api" })
            {
                var microserviceDirectory = $"{request.Directory}{Path.DirectorySeparatorChar}{name}{Path.DirectorySeparatorChar}{name}.{layer}";

                var microservice = new ProjectModel { Name = $"{name}.{layer}", Directory = microserviceDirectory , DotNetProjectType = DotNetProjectType.ClassLib };

                microservice.References = new List<string>
                {
                    @"..\..\..\BuildingBlocks\Messaging\Messaging.Udp\Messaging.Udp.csproj"
                };

                if (layer == "Infrastructure")
                {
                    microservice.References.Add(@$"..\{name}.Core\{name}.Core.csproj");
                }

                if (layer == "Api")
                {
                    microservice.References.Add(@$"..\{name}.Infrastructure\{name}.Infrastructure.csproj");
                    microservice.DotNetProjectType = DotNetProjectType.Web;
                }

                _projectService.AddProject(microservice);

                _projectService.AddProject(new ProjectModel
                {
                    Name = $"{name}.{layer}.Tests",
                    Directory = $"{request.Directory}{Path.DirectorySeparatorChar}{name}{Path.DirectorySeparatorChar}{name}.{layer}.Tests",
                    DotNetProjectType = DotNetProjectType.XUnit,
                    References = new List<string>() { @$"..\{name}.{layer}\{name}.{layer}.csproj" }
                });
            }
        }

        return new();
    }
}