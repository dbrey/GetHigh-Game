using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SmoothCameraMenu : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] float duration;
    [SerializeField] RectTransform rt;
    [SerializeField] AnimationCurve curva; 

    public void MoveDown()
    {
        Smooth(distance).Forget();
    }

    public void MoveUp()
    {
        Smooth(-distance).Forget(); 
    }

    async UniTaskVoid Smooth(float d)
    {
        SignalBus<PlaySoundSignal>.Fire(new PlaySoundSignal(Sounds.BotonYApuntes));

        float time = 0;
        Vector3 iniPos = rt.anchoredPosition; 
        Vector3 lastPos = new Vector3(rt.anchoredPosition.x, rt.anchoredPosition.y + d, 0); 

        while(time < duration)
        {
            rt.anchoredPosition = Vector3.Lerp(iniPos, lastPos, curva.Evaluate(time/duration)); 
            time += Time.deltaTime;
            await UniTask.Yield(); 
        }
    }
}
