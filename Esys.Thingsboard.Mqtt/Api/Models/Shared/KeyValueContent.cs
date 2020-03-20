using Newtonsoft.Json;
using System.Collections.Generic;

namespace Esys.Thingsboard.Mqtt.Api.Models.Shared
{
    [JsonDictionary]
    public class KeyValueContent : Dictionary<string, object>
    {
    }
}
