using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager sharedInstance;
    public Text titleLabel;
    public Text scoreLabel;
    public Text gameOverLabel;
    private int actualScore;

    void Awake()
    {
        if (sharedInstance==null)
        {
            sharedInstance = this;
        }
        actualScore = 0;
        gameOverLabel.enabled = false;
    }
    void Update()
    {

        if (!GameManager.sharedInstance.gameOver)
        {
            if (GameManager.sharedInstance.gamePaused || !GameManager.sharedInstance.gameStarted)
            {
                titleLabel.enabled = true;
            }
            else
            {
                titleLabel.enabled = false;
            }
        }
        else
        {
            titleLabel.enabled = false;
            gameOverLabel.enabled = true;
        }
        
    }
    public void addScore(int x)
    {
        actualScore += x;
        scoreLabel.text = actualScore.ToString();
    }
}
