using CommandLine;
using Endpoint.Application.Builders;
using Endpoint.Application.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Features
{
    internal class Model
    {
        [Verb("model")]
        internal class Request : IRequest<Unit>
        {
            [Option('d', Required = false)]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;

            [Value(0)]
            public string Entity { get; set; }
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ISettingsProvider _settingsProvider;

            public Handler(
                ISettingsProvider settingsProvider
                )
            {
                _settingsProvider = settingsProvider;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var settings = _settingsProvider.Get(request.Directory);

                BuilderFactory.Create((a, b, c, d) => new ModelBuilder(a, b, c, d))
                    .SetDomainDirectory(settings.DomainDirectory)
                    .SetDomainNamespace(settings.DomainNamespace)
                    .SetEntityName(request.Entity)
                    .Build();

                return new();
            }
        }
    }
}
