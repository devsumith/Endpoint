using MediatR;

namespace Endpoint.Core.Messages;

public class ServiceFileCreated: INotification {
    public ServiceFileCreated(string interfaceName, string className, string directory)
    {
        InterfaceName = interfaceName;
        ClassName = className;
        Directory = directory;
    }

    public string InterfaceName { get; set; }
    public string ClassName { get; set; }
    public string Directory { get; set; }
}
