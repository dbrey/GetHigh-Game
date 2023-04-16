using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Interfaces;
using Telemetry;

namespace Telemetry.PersistanceSystem
{
    internal abstract class PersistanceSystem : IPersistance
    {
        private readonly ISerializer serializer;

        protected PersistanceSystem(ISerializer serializer) => this.serializer = serializer;

        // Object dado que puede ser csv, json, binario o cualquier cosa
        protected abstract void Persist(object data);

        public void Persist(Event e) => Persist(serializer.Serialize(e));
    }
}

