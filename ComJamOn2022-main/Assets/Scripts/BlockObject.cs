using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BlockObject : MonoBehaviour
{
    [SerializeField]float newScale;
    [SerializeField] AnimationCurve curve;
    Vector2 initialScale;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    Collider2D collider;
    CancellationTokenSource source;

    void Start()
    {
        source = new CancellationTokenSource();
        this.gameObject.AddComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        initialScale = transform.localScale;
        rb.gravityScale = 0;
    }

    private void OnDestroy()
    {
        source.Cancel();
        source.Dispose();
    }

    private void OnMouseUp()
    {
        if (!isActiveAndEnabled) return;
        SignalBus<SignalOnBecomeVisible>.Fire(new SignalOnBecomeVisible(true));
        this.gameObject.layer = 0;
        sprite.sortingOrder = -1;
        this.enabled = false;
        collider.isTrigger = false;
        rb.gravityScale = 1;
        collider.enabled = true;
        RestoreAlpha().Forget();
        transform.SetParent(null);
        rb.isKinematic = false;
    }

    async UniTaskVoid RestoreAlpha()
    {
        float duration = 0.25f;
        float timer = 0;
        Color intialColor = sprite.color;

        while (timer < duration)
        {
            float alpha = Mathf.Lerp(0.5f, 1, curve.Evaluate(timer / duration));
            sprite.color = new Color(intialColor.r, intialColor.g, intialColor.b, alpha);
            timer += Time.deltaTime;
            await UniTask.Yield(source.Token);
        }

        if(sprite != null)sprite.color = new Color(intialColor.r, intialColor.g, intialColor.b, 1f); ;
    }

    private void OnMouseDown()
    {
        if (!isActiveAndEnabled) return;
        SignalBus<PlaySoundSignal>.Fire(new PlaySoundSignal(Sounds.BotonYApuntes));
        collider.enabled = false;
        ScaleOverTime().Forget();
    }

    public void SetNewScale(float newScale) => this.newScale = newScale;

    async UniTaskVoid ScaleOverTime()
    {
        float duration = 0.25f;
        float timer = 0;
        Vector3 finalScale = new Vector3(newScale, newScale, 1);
        Color initialColor = sprite.color;

        while (timer < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, finalScale, curve.Evaluate(timer/duration)); // 0 - 1 0 - 0.25
            float alpha = Mathf.Lerp(1, 0.5f, curve.Evaluate(timer / duration));
            sprite.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            timer += Time.deltaTime;
            await UniTask.Yield(source.Token);
        }

        if (source.Token.IsCancellationRequested)
            return;

        transform.localScale = finalScale;
        sprite.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0.5f);
    }

    private void OnMouseDrag()
    {
        if (!isActiveAndEnabled) return;
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(newPos.x, newPos.y, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector2(newScale, newScale));
    }
}
