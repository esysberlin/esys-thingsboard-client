using Esys.Thingsboard.Mqtt.Api;
using Esys.Thingsboard.Mqtt.Api.Models.Device;
using Esys.Thingsboard.Mqtt.Api.Models.Gateway;
using Esys.Thingsboard.Mqtt.Api.Models.Shared;
using Moq;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Esys.Thingsboard.Mqtt.Tests
{
    public partial class MqttClientTests : ConfigurableTests
    {
        readonly Mock<IMqttClientFactory> moqIMqttClientFactory;

        readonly MqttClient client;

        int connectedCount, disconnectedCount;
        IEnumerable<MqttApplicationMessage> publishedMessages;

        public MqttClientTests()
        {
            moqIMqttClientFactory = new Mock<IMqttClientFactory>();
            moqIMqttClientFactory.Setup(f => f.CreateMqttClient()).Returns(() =>
            {
                var mqttClientMock = new MqttClientMock();
                mqttClientMock.Connected += (_, __) => connectedCount++;
                mqttClientMock.Disconnected += (_, __) => disconnectedCount++;
                publishedMessages = mqttClientMock.PublishedMessages;
                return mqttClientMock;
            });
            client = new MqttClient(moqIMqttClientFactory.Object);
        }

        [Fact]
        public async Task StartStopTest()
        {
            await client.StartAsync();
            await Task.Delay(100);
            Assert.Equal(1, connectedCount);
            Assert.Equal(0, disconnectedCount);

            await client.StopAsync();
            await Task.Delay(100);
            Assert.Equal(1, connectedCount);
            Assert.Equal(1, disconnectedCount);
        }

        public static IEnumerable<object[]> SendTestData => new List<object[]>
        {
            new object[] { new Api.Models.Device.Attributes { { "Attribute A", "on" } } },
            new object[] { new Api.Models.Device.Telemetry { Values = { { "Value A", 1.23 } } } },
            new object[] { new Connect { Device= "Device A" } },
            new object[] { new Disconnect { Device= "Device A" } },
            new object[] { new Api.Models.Gateway.Attributes { { "Device A", new KeyValueContent {  { "Attribute A", "on" } } } } },
            new object[] { new Api.Models.Gateway.Telemetry { { "Device A", new List<TelemetryData> { new TelemetryData { Values = { { "Value A", 1.23 } } } } } } },
        };

        [Theory]
        [MemberData(nameof(SendTestData))]
        public async Task SendTest(IMessage message)
        {
            await client.StartAsync();
            await client.SendAsync(message);
            await Task.Delay(100);
            var expected = new (string, string)[] { (message.Topic, JsonConvert.SerializeObject(message)) };
            var actual = publishedMessages.Select(m => (Topic: m.Topic, m.ConvertPayloadToString()));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(SendTestData))]
        public async Task SendRealTest(IMessage message)
        {
            int serverPort = Convert.ToInt32(configuration["ServerPort"]);
            var server = new MqttTestServer { Port = serverPort };
            await server.StartAsync();
            try
            {
                var client = new MqttClient() { Server = "localhost", Port = serverPort, AccessToken = string.Empty };
                await client.StartAsync();
                await client.SendAsync(message);
                await Task.Delay(1000);
                var expected = new (string, string)[] { (message.Topic, JsonConvert.SerializeObject(message)) };
                var actual = server.AcceptedMessages.Select(m => (Topic: m.Topic, m.ConvertPayloadToString()));
                Assert.Equal(expected, actual);
            }
            finally
            {
                await server.StopAsync();
            }
        }

    }
}
