using Esys.Thingsboard.Mqtt.Api.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Esys.Thingsboard.Mqtt.Api.Models.Gateway
{
    [JsonDictionary]
    public class Attributes : Dictionary<string, KeyValueContent>, IMessage
    {
        public string Topic => "v1/gateway/attributes";
    }
}
