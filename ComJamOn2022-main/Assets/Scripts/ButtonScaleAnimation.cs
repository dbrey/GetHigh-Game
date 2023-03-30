using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using UnityEngine;

public class ButtonScaleAnimation : MonoBehaviour
{
    [SerializeField] float scaleChange = 2.0f;
    [SerializeField] float duration = 0.5f;
    [SerializeField] AnimationCurve curve;

    CancellationTokenSource source;

    public void OnPointerDown()
    {
        Scale(source.Token).Forget();
    }

    async UniTaskVoid Scale(CancellationToken token)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 initialScale = rectTransform.localScale;
        Vector3 finalScale = rectTransform.localScale - (Vector3)(Vector2.one *scaleChange);
        float time = 0;

        while (time < duration)
        {
            rectTransform.localScale = Vector3.Lerp(initialScale, finalScale, curve.Evaluate(time / duration));
            time += Time.deltaTime;
            await UniTask.Yield(token);
        }

        rectTransform.localScale = initialScale;
    }

    void Start()
    {
        source = new CancellationTokenSource();
    }

    private void OnDestroy()
    {
        source.Cancel();
        source.Dispose();
    }

}
