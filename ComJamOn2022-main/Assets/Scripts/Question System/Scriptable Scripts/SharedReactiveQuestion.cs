using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Question System/Shared Question", fileName = "Shared Question")]
public class SharedReactiveQuestion : ScriptableObject
{
    private Question[] questions;
    private int currentQuestion = 0;

    public void Initialize(QuestionAssetGenerator generator)
    {
        questions = generator.GetShuffleQuestions();
        currentQuestion = 0;
    }

    public event Action<bool> OnQuestionAnswered;

    private Answer[] GetAnswers(int index) => questions[index].answers;

    public void AnswerQuestion(int answerIndex)
    {
        if(currentQuestion >= questions.Length || answerIndex >= GetAnswers(currentQuestion).Length) 
        {
            Debug.LogWarning("Estas intentando acceder a una pregunta o respusta fuera del array");
            return;
        }

        var selectedAnswer = GetAnswers(currentQuestion)[answerIndex];
        OnQuestionAnswered?.Invoke(GetAnswers(currentQuestion)[answerIndex].isCorrect);
    }

    public void NextQuestion()
    {
        currentQuestion++;
        if (currentQuestion >= questions.Length)
        {
            questions.ShuffleQuestions();
            currentQuestion = 0;
        }
    }

    public int GetQuestionIndex => currentQuestion;
    public Question GetCurrentQuestion => questions[currentQuestion];
    private void OnEnable()
    {
        questions = new Question[0];
        currentQuestion = 0;
    }
}
