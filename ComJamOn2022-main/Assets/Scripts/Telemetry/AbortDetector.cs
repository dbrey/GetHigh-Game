using System.Collections;
using System.Collections.Generic;
using Telemetry;
using UnityEngine;

public class AbortDetector : MonoBehaviour
{
    // Este script estara en la escena de juego. No se puede salir de forma normal de una partida
    // porque no hay menu, asi que si se detecta que el juego se cierra es probablemente un crasheo
    // o que el jugador lo cerró en medio de una partida.

    // Hay otro script que tambien usa este evento para controlar el fin de sesión, pero este se ejecuta antes.
    private void OnApplicationQuit()
    {
        Tracker.Instance.TrackEvent(new GameAborted());
    }
}
