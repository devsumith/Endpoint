using MediatR;

namespace Endpoint.Core.Messages;

public class MessageCreated: INotification
{
    private readonly string _name;

    public MessageCreated(string name){
        _name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string Name { get; set; }
}

