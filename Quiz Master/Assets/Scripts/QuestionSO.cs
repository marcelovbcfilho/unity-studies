using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "Quiz Question", order = 0)]
public class QuestionSO : ScriptableObject
{
    [TextArea(minLines: 2, maxLines: 6)]
    [SerializeField] private string question = "Enter new question here";
    [SerializeField] private string[] answers = new string[4];
    [SerializeField] private int correctAnswerIndex;

    public string GetQuestion()
    {
        return this.question;
    }

    public string GetAnswer(int index)
    {
        return this.answers[index];
    }

    public int GetCorrectAnswerIndex()
    {
        return this.correctAnswerIndex;
    }
}
