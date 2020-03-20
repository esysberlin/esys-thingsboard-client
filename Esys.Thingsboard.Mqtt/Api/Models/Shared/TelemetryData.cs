using Newtonsoft.Json;
using System;

namespace Esys.Thingsboard.Mqtt.Api.Models.Shared
{
    public class TelemetryData
    {
        [JsonProperty("ts", Order = 1)]
        private long ts;

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
