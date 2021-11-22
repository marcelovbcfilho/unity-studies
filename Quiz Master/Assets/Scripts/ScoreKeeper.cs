using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private int correctAnswers = 0;
    private int questionsSeen = 0;

    public int GetCorrectAnswers()
    {
        return this.correctAnswers;
    }

    public void IncrementCorrectAnswers()
    {
        this.correctAnswers++;
    }

    public int GetQuestionsSeen()
    {
        return this.questionsSeen;
    }

    public void IncrementQuestionsSeen()
    {
        this.questionsSeen++;
    }

    public int CalculateScore()
    {
        return Mathf.RoundToInt(this.correctAnswers / (float)this.questionsSeen * 100);
    }
}
