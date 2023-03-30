using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfVisible : MonoBehaviour
{
    private bool enabled = false;
    public bool startVisible = false;
    public bool SetEnabled(bool enabled) => this.enabled = enabled;

    private void OnBecameInvisible()
    {
        if(enabled || startVisible)SignalBus<SignalOnBecomeVisible>.Fire(new SignalOnBecomeVisible(false));
    }

    private void OnBecameVisible()
    {
        if(enabled || startVisible)SignalBus<SignalOnBecomeVisible>.Fire(new SignalOnBecomeVisible(true));
    }
}
