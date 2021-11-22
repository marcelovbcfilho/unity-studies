using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeToCompleteQuestion = 30f;
    [SerializeField] private float timeToShowCorrectAnswer = 10f;

    private float timerValue;

    public bool loadNextQuestion;
    public float fillFraction;
    public bool isAnsweringQuestion;

    void Update()
    {
        this.UpdateTimer();
    }

    public void CancelTimer()
    {
        this.timerValue = 0;
    }

    private void UpdateTimer()
    {
        this.timerValue -= Time.deltaTime;

        if (this.isAnsweringQuestion)
        {
            if (this.timerValue > 0)
            {
                this.fillFraction = this.timerValue / this.timeToCompleteQuestion;
            }
            else
            {
                this.isAnsweringQuestion = false;
                this.timerValue = this.timeToShowCorrectAnswer;
            }
        }
        else
        {
            if (this.timerValue > 0)
            {
                this.fillFraction = this.timerValue / this.timeToShowCorrectAnswer;
            }
            else
            {
                this.isAnsweringQuestion = true;
                this.timerValue = this.timeToCompleteQuestion;
                this.loadNextQuestion = true;
            }
        }
    }
}
