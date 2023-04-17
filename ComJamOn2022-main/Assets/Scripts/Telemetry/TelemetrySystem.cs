using UnityEngine;

using Telemetry;
using System.IO;

public class TelemetrySystem : MonoBehaviour
{
    [SerializeField]
    string userId;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        File.Delete("./logs.json");
        Tracker.Init(userId);
    }

    private void OnApplicationQuit()
    {
        Tracker.End();
    }
}
