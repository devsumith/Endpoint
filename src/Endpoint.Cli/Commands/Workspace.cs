using CommandLine;
using Endpoint.Core.Strategies.Solutions.Crerate;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands
{
    internal class Workspace
    {
        [Verb("workspace")]
        internal class Request : IRequest<Unit> {
            [Option('n',"name")]
            public string Name { get; set; }
            [Option('d',"directory")]
            public string Directory { get; set; }
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ILogger _logger;
            private readonly IWorkspaceGenerationStrategyFactory _workspaceGenerationStrategyFactory;

            public Handler(ILogger logger, IWorkspaceGenerationStrategyFactory workspaceGenerationStrategyFactory)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _workspaceGenerationStrategyFactory = workspaceGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(workspaceGenerationStrategyFactory));
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"Handled: {nameof(Workspace)}");

/*                var model = Wro.Create(request.Name, request.Directory);

                _workspaceGenerationStrategyFactory.CreateFor(model);
*/
                return new();
            }
        }
    }
}
