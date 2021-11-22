using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalScoreText;
    private ScoreKeeper scoreKeeper;

    void Awake()
    {
        this.scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void ShowFinalScore()
    {
        this.finalScoreText.text = "Parabens ?!\nScore: " + this.scoreKeeper.CalculateScore() + "%";
    }
}
