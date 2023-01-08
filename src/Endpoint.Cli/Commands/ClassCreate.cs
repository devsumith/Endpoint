using CommandLine;
using Endpoint.Core.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Endpoint.Cli.Commands;


[Verb("class-create")]
public class ClassCreateRequest : IRequest<Unit>
{

    [Option('n')]
    public string Name { get; set; }

    [Option('p')]
    public string Properties { get; set; }

    [Option('d')]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ClassCreateRequestHandler : IRequestHandler<ClassCreateRequest, Unit>
{
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;

    public ClassCreateRequestHandler(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory)
    {
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory;
    }

    public Task<Unit> Handle(ClassCreateRequest request, CancellationToken cancellationToken)
    {


        return Task.FromResult(new Unit());
    }
}
