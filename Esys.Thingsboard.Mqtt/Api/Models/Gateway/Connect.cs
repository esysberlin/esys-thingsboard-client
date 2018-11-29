using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esys.Thingsboard.Mqtt.Api.Models.Gateway
{
    public class Connect : ITopicMessage
    {
        public string Topic => "v1/gateway/connect";

        [JsonProperty("device")]
        public string Device { get; set; }
    }
}
