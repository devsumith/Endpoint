// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Reactive.Linq;

namespace Messaging.Udp;

public class MessagingClient: IMessagingClient
{
    private readonly ILogger<MessagingClient> _logger;
    private readonly IServiceBusMessageListener _serviceBusMessageListener;

    public MessagingClient(
        ILogger<MessagingClient> logger,
        IServiceBusMessageListener serviceBusMessageListener){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceBusMessageListener = serviceBusMessageListener ?? throw new ArgumentNullException(nameof(serviceBusMessageListener));

    }

    public async Task<IServiceBusMessage> ReceiveAsync(ReceiveRequest receiveRequest)
    {
        var tcs = new TaskCompletionSource<IServiceBusMessage>();

        _serviceBusMessageListener.Subscribe(tcs.SetResult);

        return await tcs.Task;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _serviceBusMessageListener.StartAsync(cancellationToken);
    }

}

