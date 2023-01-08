using CommandLine;
using Endpoint.Core.Builders;
using Endpoint.Core.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;

[Verb("unit-test")]
public class UnitTestAddRequest : IRequest<Unit>
{
    [Value('n')]
    public string Name { get; set; }

    [Option('d')]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class UnitTestAddRequestHandler : IRequestHandler<UnitTestAddRequest, Unit>
{
    private readonly IContext _context;
    private readonly IFileSystem _fileSystem;
    private readonly ISettingsProvider _settingsProvider;

    public UnitTestAddRequestHandler(IContext context, IFileSystem fileSystem, ISettingsProvider settingsProvider)
    {
        _context = context;
        _fileSystem = fileSystem;
        _settingsProvider = settingsProvider;
    }

    public Task<Unit> Handle(UnitTestAddRequest request, CancellationToken cancellationToken)
    {
        var settings = _settingsProvider.Get();

        new UnitTestBuilder(_context, _fileSystem)
            .WithName(request.Name)
            .WithRootNamespace(settings.RootNamespace)
            .WithDirectory(request.Directory)
            .Build();

        return Task.FromResult(new Unit());
    }
}
