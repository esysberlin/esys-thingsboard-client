using Esys.Thingsboard.Mqtt.Api.Models.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esys.Thingsboard.Mqtt.Api.Models.Gateway
{
    [JsonDictionary]
    public class Telemetry : Dictionary<string, List<TelemetryData>>, IMessage
    {
        public string Topic => "v1/gateway/telemetry";
    }
}
