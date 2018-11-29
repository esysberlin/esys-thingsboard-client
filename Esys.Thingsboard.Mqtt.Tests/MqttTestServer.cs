using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Esys.Thingsboard.Mqtt.Tests
{
    public class MqttTestServer
    {
        IMqttServer mqttServer;

        public int? Port { get; set; }

        ConcurrentBag<MqttApplicationMessage> acceptedMessages = new ConcurrentBag<MqttApplicationMessage>();

        public IEnumerable<MqttApplicationMessage> AcceptedMessages { get => acceptedMessages.ToArray(); }

        public async Task StartAsync()
        {
            mqttServer = new MqttFactory().CreateMqttServer();
            var options = new MqttServerOptionsBuilder()
                .WithDefaultEndpointPort(Port ?? 1888)
                .WithApplicationMessageInterceptor(c =>
                {
                    acceptedMessages.Add(c.ApplicationMessage);
                })
                .Build();
            await mqttServer.StartAsync(options);
        }

        public async Task StopAsync()
        {
            await mqttServer.StopAsync();
        }
    }
}
