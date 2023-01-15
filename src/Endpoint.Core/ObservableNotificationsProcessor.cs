using Endpoint.Core.Internals;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Core;

public class ObservableNotificationsProcessor: BackgroundService
{
    private readonly ILogger<ObservableNotificationsProcessor> _logger;
    private readonly IMediator _mediator;
    private readonly Observable<INotification> _observableNotifications;

    public ObservableNotificationsProcessor(ILogger<ObservableNotificationsProcessor> logger, Observable<INotification> observableNotifications, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _observableNotifications = observableNotifications ?? throw new ArgumentNullException(nameof(observableNotifications));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var tcs = new TaskCompletionSource<INotification>(TaskCreationOptions.RunContinuationsAsynchronously);

        _observableNotifications.Subscribe(async message =>
        {

            await _mediator.Publish(message);
        });

        await tcs.Task;

    }

}
