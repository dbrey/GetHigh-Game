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
        Tracker.Instance.TrackEvent(new StartSession());
    }

    private void OnApplicationQuit()
    {
        Tracker.Instance.TrackEvent(new EndSession());
        Tracker.End();
    }
}
