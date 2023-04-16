using UnityEngine;

using Telemetry;

public class TelemetrySystem : MonoBehaviour
{
    [SerializeField]
    string userId;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Tracker.Init(userId);
    }
    
    private void OnApplicationQuit() => Tracker.End();
}
