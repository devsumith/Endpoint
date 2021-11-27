using CommandLine;
using Endpoint.Application.Builders;
using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Features
{
    internal class GetById
    {
        [Verb("get-by-id")]
        internal class Request : IRequest<Unit>
        {
            [Value(0)]
            public string Entity { get; set; }

            [Option('d')]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ISettingsProvider _settingsProvder;
            private readonly IFileSystem _fileSystem;

            public Handler(ISettingsProvider settingsProvider, IFileSystem fileSystem)
            {
                _settingsProvder = settingsProvider;
                _fileSystem = fileSystem;
            }

            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var settings = _settingsProvder.Get(request.Directory);

                new GetByIdBuilder(new Context(), _fileSystem)
                    .WithDirectory($"{settings.ApplicationDirectory}{Path.DirectorySeparatorChar}Features{Path.DirectorySeparatorChar}{((Token)request.Entity).PascalCasePlural}")
                    .WithDbContext(settings.DbContext)
                    .WithNamespace($"{settings.ApplicationNamespace}.Features")
                    .WithApplicationNamespace($"{settings.ApplicationNamespace}")
                    .WithDomainNamespace($"{settings.DomainNamespace}")
                    .WithEntity(request.Entity)
                    .Build();

                return Task.FromResult(new Unit());
            }
        }
    }
}
