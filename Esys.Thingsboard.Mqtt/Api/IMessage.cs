using Newtonsoft.Json;

namespace Esys.Thingsboard.Mqtt.Api
{
    public interface IMessage
    {
        [JsonIgnore]
        string Topic { get; }
    }
}
