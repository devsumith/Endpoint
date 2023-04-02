namespace Messaging;

public interface IServiceBusMessageListener : IObservable<IServiceBusMessage>
{
    Task StartAsync(CancellationToken cancellationToken = default);
}