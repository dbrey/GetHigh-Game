using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Telemetry;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlocksSystem : MonoBehaviour
{
    [SerializeField] QuestionAssetGenerator questionGenerator;
    [SerializeField] QuestionAssetGenerator rockQuestions;
    [SerializeField] SharedReactiveQuestion sharedQuestion;
    [SerializeField] SharedReactiveQuestion rockSharedQuestion;
    [SerializeField] GameObject blocksPrefabs;
    [SerializeField] AnimationCurve curve;
    [SerializeField] Transform blockInventory;
    [SerializeField] Sprite[] blockSprites;
    int numOfCubes;

    int telBlkCount = 0;

    [SerializeField] Bounds blockSpawnArea;
    [SerializeField] Vector3 origin;
    Result result;
    Camera cam;

    Vector3 GetOriginPoint() => blockSpawnArea.center + Camera.main.transform.position + origin;
    Vector3 GetDestinationPoint(Vector3 point) => point + Camera.main.transform.position;

    private void Awake()
    {
        cam = Camera.main;
        result = FindObjectOfType<Result>();
        sharedQuestion.Initialize(questionGenerator);
        rockSharedQuestion.Initialize(rockQuestions);
        SignalBus<SignalOnBecomeVisible>.Subscribe(OnCubeChange);
    }

    public void SpawnBlock()
    {
        Vector3 pointInBounds = blockSpawnArea.RandomPointInBounds();
        var instantiated = Instantiate(blocksPrefabs, GetOriginPoint(), Quaternion.identity);
        instantiated.GetComponent<SpriteRenderer>().sprite = blockSprites.GetRandom();
        instantiated.transform.parent = blockInventory;
        instantiated.GetComponent<BlockObject>().SetNewScale(Random.Range(1, 2.5f));
        instantiated.GetComponent<BlockObject>().SetBlkId(telBlkCount);
        MoveSpawnCube(instantiated.transform, pointInBounds).Forget();

        Tracker.Instance.TrackEvent(new GetBlock(telBlkCount++));
    }

    async UniTaskVoid MoveSpawnCube(Transform transform, Vector3 point)
    {
        float dur = 0.25f;
        float timer = 0;
        Vector3 endPoint = new Vector3(GetDestinationPoint(point).x, GetDestinationPoint(point).y,0);


        while (timer < dur)
        {
            transform.position = Vector3.Lerp(GetOriginPoint(), endPoint, curve.Evaluate(timer / dur));
            timer += Time.deltaTime;
            await UniTask.Yield();
        }

        transform.position = endPoint;
        transform.GetComponent<CheckIfVisible>().SetEnabled(true);
    }
    [SerializeField] UITransicion transition;

    bool finished = false;
    float countTime = 0;
    private void Update()
    {
        countTime += Time.deltaTime;
    }

    void OnCubeChange(SignalOnBecomeVisible context)
    {
        if (context.isVisible) numOfCubes++;
        else numOfCubes--;

        if (numOfCubes <= 0)
        {
            result.TotalTime = Mathf.FloorToInt(countTime);
            Debug.Log("Time :" + result.TotalTime);

            if (!finished)
                FinishGame().Forget();
        }
    }

    async UniTaskVoid FinishGame()
    {
        finished = true; 
        await transition.EndLerp();
        Tracker.Instance.TrackEvent(new EndGame());
        SceneManager.LoadScene("Game Over");
    }

    private void OnDestroy()
    {
        SignalBus<SignalOnBecomeVisible>.Unsubscribe(OnCubeChange);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(blockSpawnArea.center + Camera.main.transform.position, blockSpawnArea.size);
        Gizmos.DrawWireSphere(blockSpawnArea.center + Camera.main.transform.position + origin, 0.25f);
    }
}
