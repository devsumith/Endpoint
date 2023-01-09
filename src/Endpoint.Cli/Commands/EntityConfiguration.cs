using CommandLine;
using Endpoint.Core.Builders;
using Endpoint.Core.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;

public class EntityConfiguration
{
    [Verb("entity-config")]
    public class Request : IRequest<Unit>
    {
        [Value(0)]
        public string Entity { get; set; }

        [Option('d', Required = false)]
        public string Directory { get; set; } = System.Environment.CurrentDirectory;
    }

    public class Handler : IRequestHandler<Request, Unit>
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly IFileSystem _fileSystem;

        public Handler(ISettingsProvider settingsProvider, IFileSystem fileSystem)
        {
            _settingsProvider = settingsProvider;
            _fileSystem = fileSystem;
        }

        public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            var settings = _settingsProvider.Get();

            new EntityConfigurationBuilder(request.Entity, settings.InfrastructureNamespace, settings.DomainNamespace, request.Directory, _fileSystem)
                .Build();

            return Task.FromResult(new Unit());
        }
    }
}
