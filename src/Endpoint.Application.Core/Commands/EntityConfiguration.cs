using CommandLine;
using Endpoint.Application.Builders;
using Endpoint.SharedKernal.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Commands
{
    internal class EntityConfiguration
    {
        [Verb("entity-config")]
        internal class Request : IRequest<Unit>
        {
            [Value(0)]
            public string Entity { get; set; }

            [Option('d', Required = false)]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
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
}
