using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Powerups { Chuleta, Cafe, Repo, DuermeBien, BebidaEnergetica}

[System.Serializable]
public struct Answer
{
    public string answer;
    public bool isCorrect;
    public Powerups powerup;
}

[System.Serializable]
public struct Question
{
    public string questionText;
    public float questionDuration;
    public Sprite profeSprite;
    public Answer[] answers;

    public void ShuffleAnswers()
    {
        answers.Shuffle();
    }
}

[CreateAssetMenu(menuName ="Question System/Question", fileName ="Question")]
public class QuestionAssetGenerator : ScriptableObject
{
    [SerializeField]private Question[] questions;
    public Question[] GetShuffleQuestions()
    {
        Question[] questions = this.questions.ToArray();
        questions.ShuffleQuestions();
        return questions;
    }
}

public static class ArrayExtentions
{
    public static void ShuffleQuestions(this Question[] questions)
    {
        questions.Shuffle();
        foreach (Question question in questions) question.ShuffleAnswers();
    }

    public static void Shuffle<T>(this T[] array)
    {
        int i = array.Length;
        while (i > 1)
        {
            int rand = UnityEngine.Random.Range(0, i--);
            T temp = array[i];
            array[i] = array[rand];
            array[rand] = temp;
        }
    }
}