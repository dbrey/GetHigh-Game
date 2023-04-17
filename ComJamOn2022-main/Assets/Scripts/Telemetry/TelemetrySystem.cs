using UnityEngine;

using Telemetry;
using Telemetry.PersistanceSystem;
using Telemetry.SerializationSystem;
using System.IO;

public class TelemetrySystem : MonoBehaviour
{
    [SerializeField]
    string userId;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        // Por ahora solo queremos guardar la �ltima ejecuci�n del tracker
        File.Delete("./logs.json");
        File.Delete("./logs.csv");
        Tracker.Init(userId);
        Tracker.Instance.AddPersistanceSystem(new FilePersistance(new CsvSerializer(), "./logs.csv"));
    }

    private void OnApplicationQuit()
    {
        Tracker.End();
    }
}
