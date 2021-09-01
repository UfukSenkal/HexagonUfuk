using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] int _scoreMult = 5;
    [SerializeField] string scoreString = "Score: ";
    int _score = 0;

    public int Score { get => _score; set => _score = value; }
    public int ScoreMult { get => _scoreMult; set => _scoreMult = value; }


    public void ScoreTextUpdate()
    {
        _scoreText.text = scoreString + Score; 
    }
}
