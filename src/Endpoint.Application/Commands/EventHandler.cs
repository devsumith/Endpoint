using CommandLine;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Commands
{
    internal class EventHandler
    {
        [Verb("event-handler")]
        internal class Request : IRequest<Unit>
        {

        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Unit());
            }
        }
    }
}
