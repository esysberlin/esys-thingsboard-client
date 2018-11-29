using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esys.Thingsboard.Mqtt.Api.Models.Shared
{
    public class TelemetryData
    {
        [JsonProperty("ts", Order = 1)]
        long ts;

        [JsonIgnore]
        public DateTimeOffset Timestamp
        {
            get => DateTimeOffset.FromUnixTimeMilliseconds(ts);
            set => ts = value.ToUnixTimeMilliseconds();
        }

        [JsonProperty("values", Order = 2)]
        public KeyValueContent Values = new KeyValueContent();
    }
}
