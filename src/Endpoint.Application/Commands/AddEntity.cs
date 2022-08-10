using CommandLine;
using Endpoint.Core.Factories;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Files.Create;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Commands
{
    internal class AddEntity
    {
        [Verb("add-entity")]
        internal class Request : IRequest<Unit> {
            [Option('n',"name")]
            public string Name { get; set; }
            [Option('p', "properties")]
            public string Properties { get; set; }
            [Option('d', "directory")]
            public string Directory { get; set; } = Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ILogger _logger;
            private readonly IFileGenerationStrategyFactory _fileGenerationStrategyFactory;
            private readonly IFileNamespaceProvider _fileNamespaceProvider;

            public Handler(ILogger logger, IFileGenerationStrategyFactory fileGenerationStrategyFactory, IFileNamespaceProvider fileNamespaceProvider)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _fileGenerationStrategyFactory = fileGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(fileGenerationStrategyFactory));
                _fileNamespaceProvider = fileNamespaceProvider ?? throw new ArgumentNullException(nameof(fileNamespaceProvider));
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"Handled: {nameof(AddEntity)}");

                var model = EntityFileModelFactory.Create(request.Name, request.Properties, request.Directory, _fileNamespaceProvider.Get(request.Directory));

                _fileGenerationStrategyFactory.CreateFor(model);

                return new();
            }
        }
    }
}
