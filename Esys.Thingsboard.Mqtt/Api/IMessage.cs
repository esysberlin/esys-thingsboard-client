using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esys.Thingsboard.Mqtt.Api
{
    public interface IMessage
    {
        [JsonIgnore]
        string Topic { get; }
    }
}
