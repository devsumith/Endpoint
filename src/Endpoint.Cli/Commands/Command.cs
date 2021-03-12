using CommandLine;
using Endpoint.Cli.Builders;
using Endpoint.Cli.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using static Endpoint.Cli.Builders.BuilderFactory;

namespace Endpoint.Cli.Commands
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

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var settings = _settingsProvder.Get(request.Directory);

                Create<CommandBuilder>((a, b, c, d) => new(a, b, c, d))
                    .SetDirectory(request.Directory)
                    .SetRootNamespace(settings.RootNamespace)
                    .WithEntity(request.Entity)
                    .WithName(request.Name)
                    .WithDbContext(settings.DbContext)
                    .SetApplicationNamespace(settings.ApplicationNamespace)
                    .SetDomainNamespace(settings.DomainNamespace)
                    .Build();

                return new();
            }
        }
    }
}
