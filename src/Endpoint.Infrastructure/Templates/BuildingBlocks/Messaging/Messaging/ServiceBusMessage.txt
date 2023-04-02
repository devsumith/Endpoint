namespace Messaging;

public class ServiceBusMessage : IServiceBusMessage
{

    public ServiceBusMessage(IDictionary<string, string> messageAttributes, string body)
    {
        MessageAttributes = messageAttributes;
        Body = body;
    }

    public IDictionary<string, string> MessageAttributes { get; init; }
    public string Body { get; init; }
}