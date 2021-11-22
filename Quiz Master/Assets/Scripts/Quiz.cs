using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private List<QuestionSO> questions = new List<QuestionSO>();
    private QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] private GameObject[] answerButtons = new GameObject[4];
    private bool hasAnsweredEarly = true;

    [Header("Button colors")]
    [SerializeField] private Sprite defaultAnswerSprite;
    [SerializeField] private Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] private Image timerImage;
    [SerializeField] private Timer timer;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;
    public bool isComplete = false;

    void Awake()
    {
        this.timer = FindObjectOfType<Timer>();
        this.scoreKeeper = FindObjectOfType<ScoreKeeper>();
        this.progressBar.maxValue = this.questions.Count;
        this.progressBar.value = 0;
    }

    void Update()
    {
        this.timerImage.fillAmount = this.timer.fillFraction;
        if (this.timer.loadNextQuestion)
        {
            if (this.progressBar.value == this.progressBar.maxValue)
            {
                this.isComplete = true;
                return;
            }

            this.hasAnsweredEarly = false;
            this.GetNextQuestion();
            this.timer.loadNextQuestion = false;
        }
        else if (!this.hasAnsweredEarly && !this.timer.isAnsweringQuestion)
        {
            this.DisplayAnswer(-1);
            this.SetButtonState(false);
        }
    }

    private void DisplayQuestion()
    {
        this.questionText.text = this.currentQuestion.GetQuestion();

        for (int i = 0; i < this.answerButtons.Length; i++)
        {
            this.answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = this.currentQuestion.GetAnswer(i);
        }
    }

    private void SetButtonState(bool state)
    {
        for (int i = 0; i < this.answerButtons.Length; i++)
        {
            this.answerButtons[i].GetComponentInChildren<Button>().interactable = state;
        }
    }

    private void GetNextQuestion()
    {
        if (this.questions.Count > 0)
        {
            this.SetButtonState(true);
            this.SetDefaultButtonSprites();
            this.GetRandomQuestion();
            this.DisplayQuestion();
            this.progressBar.value++;
            this.scoreKeeper.IncrementQuestionsSeen();
        }
    }

    private void GetRandomQuestion()
    {
        int index = UnityEngine.Random.Range(0, this.questions.Count);
        this.currentQuestion = this.questions[index];

        if (this.questions.Contains(this.currentQuestion))
            this.questions.RemoveAt(index);
    }

    private void SetDefaultButtonSprites()
    {
        for (int i = 0; i < this.answerButtons.Length; i++)
        {
            this.answerButtons[i].GetComponentInChildren<Image>().sprite = this.defaultAnswerSprite;
        }
    }

    public void OnAnswerSelected(int index)
    {
        this.hasAnsweredEarly = true;
        this.DisplayAnswer(index);
        this.SetButtonState(false);
        this.timer.CancelTimer();
        this.scoreText.text = "Score: " + this.scoreKeeper.CalculateScore() + "%";
    }

    private void DisplayAnswer(int index)
    {
        if (this.currentQuestion.GetCorrectAnswerIndex() == index)
        {
            this.questionText.text = "Acerto!!!";
            this.answerButtons[index].GetComponent<Image>().sprite = this.correctAnswerSprite;
            this.scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            this.questionText.text = "Burrao em...";
            this.answerButtons[this.currentQuestion.GetCorrectAnswerIndex()].GetComponent<Image>().sprite = this.correctAnswerSprite;
        }
    }
}
