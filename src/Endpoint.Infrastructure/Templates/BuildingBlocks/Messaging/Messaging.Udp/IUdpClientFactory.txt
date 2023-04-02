using System.Net.Sockets;

namespace Messaging.Udp;

public interface IUdpClientFactory
{
     UdpClient Create();
}