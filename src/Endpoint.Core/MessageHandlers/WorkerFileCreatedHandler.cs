using Endpoint.Core.Messages;
using Endpoint.Core.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Core.MessageHandlers;

public class WorkerFileCreatedHandler : INotificationHandler<WorkerFileCreated>
{
    private readonly IDependencyInjectionService _dependencyInjectionService;

    public WorkerFileCreatedHandler(IDependencyInjectionService dependencyInjectionService)
    {
        _dependencyInjectionService = dependencyInjectionService ?? throw new ArgumentNullException(nameof(dependencyInjectionService));
    }

    public async Task Handle(WorkerFileCreated notification, CancellationToken cancellationToken)
    {
        
    }
}
