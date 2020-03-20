using Newtonsoft.Json;

namespace Esys.Thingsboard.Mqtt.Api.Models.Gateway
{
    public class Connect : IMessage
    {
        public string Topic => "v1/gateway/connect";

        [JsonProperty("device")]
        public string Device { get; set; }
    }
}
