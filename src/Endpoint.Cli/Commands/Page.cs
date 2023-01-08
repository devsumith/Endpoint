using CommandLine;
using Endpoint.Core.Builders;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;

[Verb("page")]
public class PageRequest : IRequest<Unit>
{
    [Value(0)]
    public string Entity { get; set; }

    [Option('d')]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class PageRequestHandler : IRequestHandler<PageRequest, Unit>
{
    private readonly ISettingsProvider _settingsProvder;
    private readonly IFileSystem _fileSystem;

    public PageRequestHandler(ISettingsProvider settingsProvider, IFileSystem fileSystem)
    {
        _settingsProvder = settingsProvider;
        _fileSystem = fileSystem;
    }

    public Task<Unit> Handle(PageRequest request, CancellationToken cancellationToken)
    {
        var settings = _settingsProvder.Get(request.Directory);

        new GetPageBuilder(new Endpoint.Core.Services.Context(), _fileSystem)
            .WithDirectory($"{settings.ApplicationDirectory}{Path.DirectorySeparatorChar}Features{Path.DirectorySeparatorChar}{((Token)request.Entity).PascalCasePlural}")
            .WithDbContext(settings.DbContextName)
            .WithNamespace($"{settings.ApplicationNamespace}.Features")
            .WithApplicationNamespace($"{settings.ApplicationNamespace}")
            .WithDomainNamespace($"{settings.DomainNamespace}")
            .WithEntity(request.Entity)
            .Build();

        return Task.FromResult(new Unit());
    }
}