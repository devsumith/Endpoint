namespace Messaging;

public interface IServiceBusMessageSender
{
    Task Send(object message);
}