using Esys.Thingsboard.Mqtt.Api.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Esys.Thingsboard.Mqtt.Api.Models.Gateway
{
    [JsonDictionary]
    public class Telemetry : Dictionary<string, List<TelemetryData>>, IMessage
    {
        public string Topic { get; set; } = "v1/gateway/telemetry";

        public string User { get; set; } = string.Empty;
    }
}
