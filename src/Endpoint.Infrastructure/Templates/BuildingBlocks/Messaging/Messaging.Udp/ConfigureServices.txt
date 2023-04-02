using Messaging;
using Messaging.Udp;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices { 

    public static void AddMessagingUdpServices(this IServiceCollection services) {
        services.AddSingleton<IMessagingClient,MessagingClient>();
        services.AddSingleton<IServiceBusMessageSender, ServiceBusMessageSender>();
        services.AddSingleton<IServiceBusMessageListener, ServiceBusMessageListener>();
        services.AddSingleton<IUdpClientFactory, UdpClientFactory>();
    }
}