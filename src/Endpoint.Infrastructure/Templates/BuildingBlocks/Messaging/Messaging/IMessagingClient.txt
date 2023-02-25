namespace Messaging;

public interface IMessagingClient
{
    Task<IServiceBusMessage> ReceiveAsync(ReceiveRequest receiveRequest);
    Task StartAsync(CancellationToken cancellationToken);
}