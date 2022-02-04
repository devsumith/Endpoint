using CommandLine;
using Endpoint.Application.Builders;
using Endpoint.SharedKernal.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using static Endpoint.Application.Builders.BuilderFactory;

namespace Endpoint.Application.Features
{
    internal class Command
    {
        [Verb("command")]
        internal class Request : IRequest<Unit>
        {

            [Value(0)]
            public string Name { get; set; }

            [Value(1)]
            public string Entity { get; set; }

            [Option('d')]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ISettingsProvider _settingsProvder;

            public Handler(ISettingsProvider settingsProvider)
                => _settingsProvder = settingsProvider;

            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var settings = _settingsProvder.Get(request.Directory);

                Create<CommandBuilder>((a, b, c, d) => new(a, b, c, d))
                    .SetDirectory(request.Directory)
                    .SetRootNamespace(settings.RootNamespace)
                    .SetApplicationNamespace(settings.ApplicationNamespace)
                    .SetDomainNamespace(settings.DomainNamespace)
                    .SetNamespace($"{settings.DomainNamespace}.Features")
                    .WithBuildingBlocksCoreNamespace(settings.BuildingBlocksCoreNamespace)
                    .WithBuildingBlocksEventStoreNamespace(settings.BuildingBlocksEventStoreNamespace)
                    .WithEntity(request.Entity)
                    .WithName(request.Name)
                    .WithDbContext(settings.DbContextName)
                    .Build();

                return Task.FromResult(new Unit());
            }
        }
    }
}
