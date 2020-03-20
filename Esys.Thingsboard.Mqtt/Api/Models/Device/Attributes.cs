using Esys.Thingsboard.Mqtt.Api.Models.Shared;

namespace Esys.Thingsboard.Mqtt.Api.Models.Device
{
    public class Attributes : KeyValueContent, IMessage
    {
        public string Topic => "v1/devices/me/attributes";
    }
}
