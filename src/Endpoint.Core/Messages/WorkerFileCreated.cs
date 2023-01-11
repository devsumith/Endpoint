using MediatR;

namespace Endpoint.Core.Messages;

public class WorkerFileCreated: INotification {
    public WorkerFileCreated(string name, string directory)
    {
        Name = name;
        Directory = directory;
    }

    public string Name { get; set; }
    public string Directory { get; set; }
}
