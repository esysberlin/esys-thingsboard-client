using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esys.Thingsboard.Mqtt.Tests
{
    public partial class MqttClientTests
    {
        private class MqttClientMock : IMqttClient
        {
            public bool IsConnected { get; private set; }

            public event EventHandler<MqttClientConnectedEventArgs> Connected;

            public event EventHandler<MqttClientDisconnectedEventArgs> Disconnected;

            public event EventHandler<MqttApplicationMessageReceivedEventArgs> ApplicationMessageReceived;

            public Task<MqttClientConnectResult> ConnectAsync(IMqttClientOptions options)
            {
                IsConnected = true;
                Connected?.Invoke(this, new MqttClientConnectedEventArgs(false));
                return Task.FromResult(new MqttClientConnectResult(false));
            }

            public Task DisconnectAsync()
            {
                var wasConnected = IsConnected;
                IsConnected = false;
                Disconnected?.Invoke(this, new MqttClientDisconnectedEventArgs(wasConnected, null));
                return Task.CompletedTask;
            }

            public void Dispose()
            {
            }

            private readonly List<MqttApplicationMessage> publishedMessages = new List<MqttApplicationMessage>();

            public IEnumerable<MqttApplicationMessage> PublishedMessages => publishedMessages;

            public Task PublishAsync(MqttApplicationMessage applicationMessage)
            {
                publishedMessages.Add(applicationMessage);
                return Task.CompletedTask;
            }

            public Task<IList<MqttSubscribeResult>> SubscribeAsync(IEnumerable<TopicFilter> topicFilters) => Task.FromResult(new MqttSubscribeResult[0] as IList<MqttSubscribeResult>);

            public Task UnsubscribeAsync(IEnumerable<string> topics) => Task.CompletedTask;
        }
    }
}
