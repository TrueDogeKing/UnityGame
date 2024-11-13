
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Points : MonoBehaviour
{
    public PlayerController player;
    public TMP_Text scoreText;
    private int score;

    // Update is called once per frame
    void Update()
    {
        score = player.GetScore();
        scoreText.text= score.ToString();
    }
}
