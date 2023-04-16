using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Telemetry;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestionSystemUiHandler : MonoBehaviour
{
    private const int TIME_BETWEEN_QUESTIONS = 800;
    private int GetQuestionDuration => Mathf.FloorToInt(sharedQuestion.GetCurrentQuestion.questionDuration * 1000);

    [SerializeField] SharedReactiveQuestion sharedQuestion;
    [SerializeField] SharedReactiveQuestion rockSharedQuestion;

    [SerializeField] private Text question;
    [SerializeField] private Text[] answers;
    [SerializeField] private Text questionTimeText;
    [SerializeField] private Image profeImage;
    [SerializeField] private GameObject questionBlockContainer;
    [SerializeField] private BlocksSystem blockSystem;
    [SerializeField] private Sprite[] powerupSprites;
    [SerializeField] private Image powerupImage;
    int timer = 0;

    int rockQuestionDelay = 9;
    int rockQuestionCurrent = 0;

    bool isRockQuestion => rockQuestionCurrent == rockQuestionDelay;

    private CancellationTokenSource source;

    public void Answer(int index)
    {
        SignalBus<SignalOnBlockPlaced>.Fire();
        if (!isRockQuestion)
        {
            sharedQuestion.AnswerQuestion(index);
            rockQuestionCurrent++;
            chuleta--;
            other--;

            if (!powerupEnabled) powerupImage.enabled = false;
        }
        else HandlePowerup(index);
    }

    int chuleta = 0;
    int x2 = 0;
    int bebida = 0;
    int other = 0;

    public async UniTaskVoid StopWatch(float time)
    {
        await UniTask.Delay(Mathf.FloorToInt(time * 1000));
        powerupImage.enabled = false;
    }

    bool powerupEnabled => chuleta > 0 || x2 > 0 || bebida > 0 || other > 0;

    void HandlePowerup(int index)
    {
        Powerups powerup = rockSharedQuestion.GetCurrentQuestion.answers[index].powerup;
        powerupImage.sprite = powerupSprites[(int)powerup];
        powerupImage.enabled = true;
        switch (powerup)
        {
            case Powerups.Chuleta:
                chuleta = 3;
                break;
            case Powerups.Cafe:
                other = 1;
                Camera.main.GetComponent<MoveCamera>().StopWatch(4.0f).Forget();
                StopWatch(4.0f).Forget();
                break;
            case Powerups.Repo:
                other = 1;
                for (int i = 0; i < 3; i++)
                    blockSystem.SpawnBlock();
                break;
            case Powerups.DuermeBien:
                x2 = 5;
                break;
            case Powerups.BebidaEnergetica:
                bebida = 5;
                break;
            default:
                break;
        }

        Debug.Log("The rock");
        rockQuestionCurrent = 0;
        OnAnswered();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        sharedQuestion.OnQuestionAnswered += OnAnswered;
        rockSharedQuestion.OnQuestionAnswered += OnAnswered;
        UpdateUI();
        source = new CancellationTokenSource();
        QuestionTimeout(source.Token).Forget();
    }

    private void Update()
    {
        timer -= (int)(Time.deltaTime * 1000);
        questionTimeText.text = Mathf.FloorToInt(timer / 1000 + 1).ToString();
    }

    void UpdateUI()
    {
        timer = GetQuestionDuration;

        Answer[] answers;
        if (isRockQuestion)
        {
            question.text = rockSharedQuestion.GetCurrentQuestion.questionText;
            answers = rockSharedQuestion.GetCurrentQuestion.answers;
        }
        else
        {
            question.text = sharedQuestion.GetCurrentQuestion.questionText;
            answers = sharedQuestion.GetCurrentQuestion.answers;
        }

        var numAnswer = answers.Length;

        for (int i = 0; i < 4; i++)
        {
            if (i < numAnswer) this.answers[i].text = answers[i].answer;
            this.answers[i].transform.parent.gameObject.SetActive(i < numAnswer);
        }

        Sprite profeSprite = isRockQuestion ? rockSharedQuestion.GetCurrentQuestion.profeSprite : sharedQuestion.GetCurrentQuestion.profeSprite;
        if (profeSprite != null) profeImage.sprite = profeSprite;
        profeImage.enabled = profeSprite != null;
        //if (isRockQuestion) rockQuestionCurrent = 0;
    }

    private void OnDestroy()
    {
        sharedQuestion.OnQuestionAnswered -= OnAnswered;
        rockSharedQuestion.OnQuestionAnswered -= OnAnswered;
        source.Cancel();
        source.Dispose();
    }

    async private UniTaskVoid QuestionTimeout(CancellationToken cancellation)
    {
        await UniTask.Delay(GetQuestionDuration, DelayType.DeltaTime, PlayerLoopTiming.Update, cancellation);
        if (!cancellation.IsCancellationRequested)
        {
            if (isRockQuestion) rockQuestionCurrent = 0;
            OnAnswered();
        }
    }

    private void OnAnswered(bool isCorrect = false)
    {
        OnAnsweredTask(isCorrect || chuleta > 0).Forget();
    }

    async private UniTaskVoid OnAnsweredTask(bool isCorrect = false)
    {
        Tracker.Instance.TrackEvent(new TimeReply(0, 0, isCorrect));

        if (isCorrect && !isRockQuestion)
        {
            blockSystem.SpawnBlock();
            if (x2 > 0) blockSystem.SpawnBlock();
            else if(!powerupEnabled) powerupImage.enabled = false;
        }
            x2--;

        if (isCorrect) SignalBus<PlaySoundSignal>.Fire(new PlaySoundSignal(Sounds.AhhCorrect));
        else SignalBus<PlaySoundSignal>.Fire(new PlaySoundSignal(Sounds.OhhhIncorrect));
        GenericExtensions.CancelAndGenerateNew(ref source);
        questionBlockContainer.SetActive(false);
        if (bebida <= 0)
            await UniTask.Delay(TIME_BETWEEN_QUESTIONS, DelayType.DeltaTime, PlayerLoopTiming.Update, source.Token);
        questionBlockContainer.SetActive(true);
        NextQuestion();
        QuestionTimeout(source.Token).Forget();

        bebida--;
        if (!powerupEnabled) powerupImage.enabled = false;

    }

    void NextQuestion()
    {
        if (!isRockQuestion) sharedQuestion.NextQuestion();
        else rockSharedQuestion.NextQuestion();
        UpdateUI();
        Tracker.Instance.TrackEvent(new TimeStart(0, 0));
    }
}
