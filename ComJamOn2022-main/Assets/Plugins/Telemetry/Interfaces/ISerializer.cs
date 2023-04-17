using System;
using Telemetry;

namespace Telemetry.Interfaces
{
    public interface ISerializer
    {
        object Serialize(Event e);
        //Event Deserialize(string data);
    }
}