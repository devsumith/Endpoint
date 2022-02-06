using CommandLine;
using Endpoint.Application.Builders;
using Endpoint.SharedKernal.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Commands
{
    internal class UnitTest
    {
        [Verb("unit-test")]
        internal class Request : IRequest<Unit>
        {
            [Value(0)]
            public string Name { get; set; }

            [Option('d')]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly IContext _context;
            private readonly IFileSystem _fileSystem;
            private readonly ISettingsProvider _settingsProvider;

            public Handler(IContext context, IFileSystem fileSystem, ISettingsProvider settingsProvider)
            {
                _context = context;
                _fileSystem = fileSystem;
                _settingsProvider = settingsProvider;
            }
            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
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
    }
}
