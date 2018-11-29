using Esys.Thingsboard.Mqtt.Api;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Esys.Thingsboard.Mqtt
{
    public class MqttClient
    {
        public MqttClient(string accessToken, string server, int? port = null)
        {
            AccessToken = accessToken;
            Server = server;
            Port = port;
        }

        public string AccessToken { get; set; }

        public string Server { get; set; }

        public int? Port { get; set; }

        readonly SemaphoreSlim clientLock = new SemaphoreSlim(1, 1);

        IManagedMqttClient client;

        public async Task StartAsync()
        {
            await clientLock.WaitAsync();
            try
            {
                var options = new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(10))
                    .WithClientOptions(new MqttClientOptionsBuilder()
                        .WithClientId(new Guid().ToString())
                        .WithTcpServer(Server, Port)
                        .WithCredentials(AccessToken)
                        .WithTls()
                        .Build())
                    .Build();
                client = new MqttFactory().CreateManagedMqttClient();
                await client.StartAsync(options);
            }
            finally
            {
                clientLock.Release();
            }
        }

        public async Task StopAsync()
        {
            await clientLock.WaitAsync();
            try
            {
                if (client == null)
                    return;
                await client.StopAsync();
            }
            finally
            {
                clientLock.Release();
            }
        }

        public async Task SendAsync(IMessage message)
        {
            await clientLock.WaitAsync();
            try
            {
                if (client == null)
                    throw new InvalidOperationException($"{nameof(MqttClient)} has to be started to send messages.");
                var mqttMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(message.Topic)
                    .WithPayload(JsonConvert.SerializeObject(message))
                    .WithExactlyOnceQoS()
                    .Build();
                await client.PublishAsync(mqttMessage);
            }
            finally
            {
                clientLock.Release();
            }
        }
    }
}
