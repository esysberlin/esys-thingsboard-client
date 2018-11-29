using Esys.Thingsboard.Mqtt.Api.Models.Device;
using Esys.Thingsboard.Mqtt.Api.Models.Gateway;
using Esys.Thingsboard.Mqtt.Api.Models.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace Esys.Thingsboard.Mqtt.Tests
{
    public class ModelSerializationTests
    {
        [Fact]
        public void SharedKeyValueContentTest()
        {
            var keyValueContent = new KeyValueContent { { "Wert A", 11.1 }, { "Wert B", -1.88 }, { "Wert C", true }, { "Wert D", 255 }, { "Wert E", "test" } };
            var json = JsonConvert.SerializeObject(keyValueContent);
            Assert.Equal("{\"Wert A\":11.1,\"Wert B\":-1.88,\"Wert C\":true,\"Wert D\":255,\"Wert E\":\"test\"}", json);
        }

        [Fact]
        public void SharedTelemetryDataTest()
        {
            var telemetryData = new TelemetryData
            {
                Timestamp = new DateTimeOffset(2018, 11, 28, 0, 0, 0, TimeSpan.Zero),
                Values = { { "Wert A", 11.1 }, { "Wert B", -1.88 }, { "Wert C", true }, { "Wert D", 255 }, { "Wert E", "test" } }
            };
            var json = JsonConvert.SerializeObject(telemetryData);
            Assert.Equal("{\"ts\":1543363200000,\"values\":{\"Wert A\":11.1,\"Wert B\":-1.88,\"Wert C\":true,\"Wert D\":255,\"Wert E\":\"test\"}}", json);
        }

        [Fact]
        public void DeviceAttributesSerializationTest()
        {
            var telemetry = new Api.Models.Device.Attributes { { "Attribute A", "on" }, { "Attribute B", true } };
            var json = JsonConvert.SerializeObject(telemetry);
            Assert.Equal("{\"Attribute A\":\"on\",\"Attribute B\":true}", json);
        }

        [Fact]
        public void DeviceTelemetrySerializationTest()
        {
            var telemetry = new Api.Models.Device.Telemetry
            {
                Timestamp = new DateTimeOffset(2018, 11, 28, 0, 0, 0, TimeSpan.Zero),
                Values = { { "Wert A", 11.1 }, { "Wert B", -1.88 } }
            };
            var json = JsonConvert.SerializeObject(telemetry);
            Assert.Equal("{\"ts\":1543363200000,\"values\":{\"Wert A\":11.1,\"Wert B\":-1.88}}", json);
        }

        [Theory]
        [InlineData("Device A")]
        public void GatewayConnectSerializationTest(string device)
        {
            var json = JsonConvert.SerializeObject(new Connect { Device = device });
            Assert.Equal($"{{\"device\":\"{device}\"}}", json);
        }

        [Theory]
        [InlineData("Device A")]
        public void GatewayDisconnectSerializationTest(string device)
        {
            var json = JsonConvert.SerializeObject(new Disconnect { Device = device });
            Assert.Equal($"{{\"device\":\"{device}\"}}", json);
        }

        [Fact]
        public void GatewayAttributesSerializationTest()
        {
            var attributes = new Api.Models.Gateway.Attributes
            {
                { "Device A", new KeyValueContent { { "Attribute A", "off" }, { "Attribute B", true } } },
                { "Device B", new KeyValueContent { { "Attribute A", "on" }, { "Attribute B", false } } }
            };
            var json = JsonConvert.SerializeObject(attributes);
            Assert.Equal("{\"Device A\":{\"Attribute A\":\"off\",\"Attribute B\":true},\"Device B\":{\"Attribute A\":\"on\",\"Attribute B\":false}}", json);
        }

        [Fact]
        public void GatewayTelemetrySerializationTest()
        {
            var telemetry = new Api.Models.Gateway.Telemetry
            {
                { "Device A", new List<TelemetryData> {
                    new TelemetryData {
                        Timestamp = new DateTimeOffset(2018, 11, 28, 0, 0, 0, TimeSpan.Zero),
                        Values = { { "Wert A", 11.1 }, { "Wert B", -1.88 } }
                    },
                    new TelemetryData {
                        Timestamp = new DateTimeOffset(2018, 11, 28, 1, 0, 0, TimeSpan.Zero),
                        Values = { { "Wert A", 13.33 }, { "Wert B", -1.75 } }
                    },
                } },
                { "Device B", new List<TelemetryData> {
                    new TelemetryData {
                        Timestamp = new DateTimeOffset(2018, 11, 28, 0, 0, 0, TimeSpan.Zero),
                        Values = { { "Wert A", 10.01 }, { "Wert B", -1.99 } }
                    },
                } }
            };
            var json = JsonConvert.SerializeObject(telemetry);
            Assert.Equal("{\"Device A\":[{\"ts\":1543363200000,\"values\":{\"Wert A\":11.1,\"Wert B\":-1.88}},{\"ts\":1543366800000,\"values\":{\"Wert A\":13.33,\"Wert B\":-1.75}}],\"Device B\":[{\"ts\":1543363200000,\"values\":{\"Wert A\":10.01,\"Wert B\":-1.99}}]}", json);
        }
    }
}
