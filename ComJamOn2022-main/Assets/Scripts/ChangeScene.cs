using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Telemetry;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ChangeScene : MonoBehaviour
{
    public void SceneGame()
    {
        GenericSceneChange("Escena Rioni").Forget();
        Tracker.Instance.TrackEvent(new StartGame());
        //SceneManager.LoadScene("Escena Rioni");
    }

    public void SceneGameNormal()
    {
        GenericSceneChange("Escena Juego Normal").Forget();
        Tracker.Instance.TrackEvent(new StartGame());
        //SceneManager.LoadScene("Escena Rioni");
    }

    public void Credits()
    {
        GenericSceneChange("Creditos").Forget();
        //SceneManager.LoadScene("Creditos");
    }

    public void MainMenu()
    {
        Debug.Log("Menu");
        GenericSceneChange("Menu Inicio").Forget();
        //SceneManager.LoadScene("Menu Inicio");
    }

    [SerializeField] UITransicion transition;
    public async UniTaskVoid GenericSceneChange(string sceneName)
    {
        SignalBus<PlaySoundSignal>.Fire(new PlaySoundSignal(Sounds.BotonYApuntes));
        await transition.EndLerp();
        SceneManager.LoadScene(sceneName);
    }
}
