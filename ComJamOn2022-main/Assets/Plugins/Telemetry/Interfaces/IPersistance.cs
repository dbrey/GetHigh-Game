using Telemetry;

namespace Telemetry.Interfaces
{
    public interface IPersistance
    {
        void Persist(Event e);
    }
}