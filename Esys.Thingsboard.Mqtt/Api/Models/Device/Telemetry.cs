using Esys.Thingsboard.Mqtt.Api.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esys.Thingsboard.Mqtt.Api.Models.Device
{
    public class Telemetry : TelemetryData, IMessage
    {
        public string Topic => "v1/devices/me/telemetry";
    }
}
