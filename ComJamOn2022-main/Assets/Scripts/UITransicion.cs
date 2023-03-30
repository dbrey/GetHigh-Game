using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UITransicion : MonoBehaviour
{
    [SerializeField] RectTransform left;
    [SerializeField] RectTransform right;
    [SerializeField] AnimationCurve curve;

    [SerializeField] float distance;
    [SerializeField] float duration;

    CancellationTokenSource sourceLeft;
    CancellationTokenSource sourceRight;

    void Start()
    {
        sourceLeft = new CancellationTokenSource();
        sourceRight = new CancellationTokenSource();
        StartLerp().Forget();
    }

    private void OnDestroy()
    {
        sourceLeft.Cancel();
        sourceRight.Cancel();

        sourceLeft.Dispose();
        sourceRight.Dispose();
    }

    async UniTaskVoid StartLerp()
    {
        await UniTask.Delay(500);
        if (left != null && right != null)
        {
            Lerp(left, left.anchoredPosition, left.anchoredPosition + Vector2.left * distance, curve, duration, sourceLeft.Token).Forget();
            Lerp(right, right.anchoredPosition, right.anchoredPosition + Vector2.right * distance, curve, duration, sourceRight.Token).Forget();
            SignalBus<PlaySoundSignal>.Fire(new PlaySoundSignal(Sounds.UITransition, 1));
        }
    }

    public async UniTask EndLerp()
    {
        await UniTask.Delay(500);
        var task1 = Lerp(left, left.anchoredPosition, left.anchoredPosition - Vector2.left * distance, curve, duration, sourceLeft.Token);
        var task2 = Lerp(right, right.anchoredPosition, right.anchoredPosition - Vector2.right * distance, curve, duration, sourceRight.Token);
        SignalBus<PlaySoundSignal>.Fire(new PlaySoundSignal(Sounds.UITransition, 1));
        await task1;
        await task2;
    }

    async UniTask Lerp(RectTransform transform, Vector3 initPos, Vector3 endPos, AnimationCurve curve, float duration, CancellationToken token)
    {
        float time = 0;

        while (time < duration)
        {
            transform.anchoredPosition = Vector3.Lerp(initPos, endPos, curve.Evaluate(time / duration));
            time += Time.deltaTime;
            await UniTask.Yield(token);
        }

       if(!token.IsCancellationRequested) transform.anchoredPosition = endPos;
    }
}
