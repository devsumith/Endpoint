using CommandLine;
using Endpoint.Core.Builders;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Endpoint.Cli.Commands;


[Verb("command-create")]
public class CommandCreateRequest : IRequest<Unit>
{

    [Value(0)]
    public string Name { get; set; }

    [Value(1)]
    public string Entity { get; set; }

    [Option('d')]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class CommandCreateRequestHandler : IRequestHandler<CommandCreateRequest, Unit>
{
    private readonly ISettingsProvider _settingsProvder;
    private readonly IFileSystem _fileSystem;

    public CommandCreateRequestHandler(ISettingsProvider settingsProvider, IFileSystem fileSystem)
    {
        _settingsProvder = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public Task<Unit> Handle(CommandCreateRequest request, CancellationToken cancellationToken)
    {
        var settings = _settingsProvder.Get(request.Directory);

        CommandBuilder.Build(settings, (Token)request.Name, new Context(), _fileSystem, request.Directory, settings.ApplicationNamespace);

        return Task.FromResult(new Unit());
    }
}
