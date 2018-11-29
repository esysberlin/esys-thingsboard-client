using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esys.Thingsboard.Mqtt.Api
{
    public interface ITopicMessage
    {
        [JsonIgnore]
        string Topic { get; }
    }
}
