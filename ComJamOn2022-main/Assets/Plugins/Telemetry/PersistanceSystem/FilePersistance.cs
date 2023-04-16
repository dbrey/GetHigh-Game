using System.IO;
using Telemetry.Interfaces;

namespace Telemetry.PersistanceSystem
{
    internal class FilePersistance : PersistanceSystem
    {
        private string path;

        public FilePersistance(ISerializer serializer, string path) : base(serializer) => this.path = path;

        //sobreescribe el meteodo Persist de PersistanceSystem
        protected override void Persist(object data)
        {
            using (StreamWriter sw = File.AppendText(path))
                sw.WriteLine(data);
        }
    }
}