using Esys.Thingsboard.Mqtt.Api.Models.Shared;

namespace Esys.Thingsboard.Mqtt.Api.Models.Device
{
    public class Telemetry : TelemetryData, IMessage
    {
        public string Topic => "v1/devices/me/telemetry";
    }
}
