using Esys.Thingsboard.Mqtt.Api.Models.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esys.Thingsboard.Mqtt.Api.Models.Gateway
{
    [JsonDictionary]
    public class Attributes : Dictionary<string, KeyValueContent>, ITopicMessage
    {
        public string Topic => "v1/gateway/attributes";
    }
}
