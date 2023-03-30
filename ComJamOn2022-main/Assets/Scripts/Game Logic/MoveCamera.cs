using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float iniSpeed;
    [SerializeField] private float endSpeed;
    [SerializeField] private float dur;
    private float timer;
    [Tooltip("Idealmente valor entre 0.01 y 0.03")]

    bool isMoving = false;
    bool started = false;

    void Start()
    {
        timer = 0.001f;
        SignalBus<SignalOnBlockPlaced>.Subscribe(OnSignalBlockPlaced);
    }

    public async UniTaskVoid StopWatch(float time)
    {
        isMoving = false;
        await UniTask.Delay(Mathf.FloorToInt(time * 1000));
        isMoving = true;
    }

    void OnSignalBlockPlaced(SignalOnBlockPlaced signal)
    {
        if(!started)
        isMoving = true;
        started = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            Vector2 currentSpeed = Vector2.up * Mathf.Lerp(iniSpeed, endSpeed, timer / dur);
            transform.Translate(currentSpeed * Time.deltaTime);
            timer += Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        SignalBus<SignalOnBlockPlaced>.Unsubscribe(OnSignalBlockPlaced);
    }
}
