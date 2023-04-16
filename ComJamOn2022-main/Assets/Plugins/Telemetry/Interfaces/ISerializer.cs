using System;
using Telemetry;

namespace Telemetry.Interfaces
{
    internal interface ISerializer
    {
        object Serialize(Event e);
        //Event Deserialize(string data);
    }
}