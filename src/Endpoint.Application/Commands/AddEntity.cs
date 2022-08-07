using CommandLine;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Endpoint.Core.Factories;

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

            public Handler(ILogger logger)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"Handled: {nameof(AddEntity)}");

                var settings = SettingsModelFactory.Resolve(request.Directory);

                // add file to Domain

                // add files to Application

                // update db context

                // add controller

                return new();
            }
        }
    }
}
