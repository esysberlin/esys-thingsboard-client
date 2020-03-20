using Newtonsoft.Json;

namespace Esys.Thingsboard.Mqtt.Api.Models.Gateway
{
    public class Disconnect : IMessage
    {
        public string Topic => "v1/gateway/disconnect";

        [JsonProperty("device")]
        public string Device { get; set; }
    }
}
