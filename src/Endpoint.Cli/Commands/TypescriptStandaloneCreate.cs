using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Artifacts.Projects;
using Endpoint.Core.Artifacts.Projects.Enums;
using Endpoint.Core.Artifacts;

namespace Endpoint.Cli.Commands;


[Verb("ts-project-create")]
public class TypescriptStandaloneCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class TypescriptStandaloneCreateRequestHandler : IRequestHandler<TypescriptStandaloneCreateRequest>
{
    private readonly ILogger<TypescriptStandaloneCreateRequestHandler> _logger;
    private readonly IArtifactGenerator _artifactGenerator;

    public TypescriptStandaloneCreateRequestHandler(ILogger<TypescriptStandaloneCreateRequestHandler> logger, IArtifactGenerator artifactGenerator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(TypescriptStandaloneCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(TypescriptStandaloneCreateRequestHandler));

        var model = new ProjectModel(DotNetProjectType.TypeScriptStandalone,request.Name,request.Directory);

        await _artifactGenerator.GenerateAsync(model);
    }
}