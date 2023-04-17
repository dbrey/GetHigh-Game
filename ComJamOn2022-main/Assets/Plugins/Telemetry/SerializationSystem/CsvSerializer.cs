using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Interfaces;

namespace Telemetry.SerializationSystem
{
    public sealed class CsvSerializer : ISerializer
    {
        private const string Delimiter = ",";
        private const string LineBreak = "\n";

        public object Serialize(Event e)
        {
            using (StringWriter sb = new StringWriter())
            {
                var properties = e.GetType().GetProperties();
                foreach (var property in properties)
                    sb.Write($"{property.Name},{property.GetValue(e)},");

                return sb.ToString().TrimEnd(',');
            }
        }
    }
}
