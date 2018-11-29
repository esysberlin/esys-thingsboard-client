using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esys.Thingsboard.Mqtt.Api.Models.Shared
{
    [JsonDictionary]
    public class KeyValueContent : Dictionary<string, object>
    {
    }
}
