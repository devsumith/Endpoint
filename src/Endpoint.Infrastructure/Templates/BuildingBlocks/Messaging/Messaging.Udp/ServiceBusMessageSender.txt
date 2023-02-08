// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Sockets;

namespace Messaging.Udp;

public class ServiceBusMessageSender: IServiceBusMessageSender
{
    private readonly ILogger<ServiceBusMessageSender> _logger;
    public ServiceBusMessageSender(
        ILogger<ServiceBusMessageSender> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Send(object message)
    {
        _logger.LogInformation("Send Message");

        var serviceBusMessage = new ServiceBusMessage(new Dictionary<string, string>()
        {
            { "MessageType", message.GetType().Name }
          
        },JsonConvert.SerializeObject(message));

        var json = JsonConvert.SerializeObject(serviceBusMessage);

        var bytesToSend = System.Text.Encoding.UTF8.GetBytes(json);

        await new UdpClient().SendAsync(bytesToSend, bytesToSend.Length, UdpClientFactory.MultiCastGroupIp, UdpClientFactory.BroadcastPort);
    }
}