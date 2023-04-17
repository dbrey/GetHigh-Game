using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Interfaces;

namespace Telemetry.SerializationSystem
{
    public sealed class JSONSerializer : ISerializer
    {
        public object Serialize(Event e) => JsonConvert.SerializeObject(e, new JsonSerializerSettings
        {
            Formatting = Formatting.None
        });
    }
}
