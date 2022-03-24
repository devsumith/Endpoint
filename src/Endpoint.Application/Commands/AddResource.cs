using CommandLine;
using Endpoint.Core.Generators;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Commands
{
    public class AddResource
    {
        [Verb("add-resource")]
        public class Request : IRequest<Unit>
        {
            [Value(0)]
            public string Resource { get; set; }

            [Option('p',"properties", Required = false)]
            public string Properties { get; set; }

            [Option('d')]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;

        }

        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ISettingsProvider _settingsProvider;
            private readonly IFileSystem _fileSystem;
            private readonly IAdditionalResourceGenerationStrategyFactory _factory;
            public Handler(
                ISettingsProvider settingsProvider,
                IAdditionalResourceGenerationStrategyFactory factory,
                IFileSystem fileSystem)
            {
                _settingsProvider = settingsProvider;
                _factory = factory;
                _fileSystem = fileSystem;
            }

            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var settings = _settingsProvider.Get(request.Directory);

                settings.AddResource(request.Resource, request.Properties, _fileSystem);

                AdditionalResourceGenerator.Generate(settings, request.Resource, request.Properties.Split(',').ToList(), request.Directory, _factory);

                return Task.FromResult(new Unit());
            }
        }
    }
}
