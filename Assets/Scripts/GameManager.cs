using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int COIN_SCORE_AMOUNT = 5;
    public static GameManager Instance { set; get; }

    public bool IsDead { set; get; }
    private bool isGameStarted = false;
    private PlayerController controller;

    //UI and UI fields
    public Animator buttonAnim;
    public Animator gameCanvas;
    public Animator menuAnim;
    public Text scoreText;
    public Text coinText;
    public Text modifierText;
    public Text highscoreText;
    private float score, coinScore, modifierScore;
    private int lastScore;

    //Death menu
    public Animator deathMenuAnim;
    public Text deadScoreText;
    public Text deadCoinText;


    private void Awake()
    {
        Instance = this;
        modifierScore = 1;
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        scoreText.text = score.ToString("0");
        coinText.text = coinScore.ToString();
        modifierText.text = "x" + modifierScore.ToString("0.0");

        highscoreText.text = PlayerPrefs.GetInt("Highscore").ToString();
    }

    private void Update()
    {
        if (MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            controller.StartRunning();
            FindObjectOfType<GlacierSpawner>().IsScrolling = true;
            FindObjectOfType<CameraController>().IsMoving = true;
            gameCanvas.SetTrigger("Show");
            menuAnim.SetTrigger("Hide");
        }

        if (isGameStarted && !IsDead)
        {
            //Bump up the score
            score += (Time.deltaTime * modifierScore);
            if (lastScore != (int)score)
            {
                lastScore = (int)score;
                Debug.Log(lastScore);
                scoreText.text = score.ToString("0");
            }
        }
    }


    public void GetCoin()
    {
        coinScore++;
        coinText.text = coinScore.ToString("0");
        score += COIN_SCORE_AMOUNT;
        scoreText.text = scoreText.text = score.ToString("0");
    }

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void OnPlayButton() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void OnDeath() 
    {
        IsDead = true;
        FindObjectOfType<GlacierSpawner>().IsScrolling = false;
        deadScoreText.text = score.ToString("0");
        deadCoinText.text = coinScore.ToString("0");
        gameCanvas.SetTrigger("Hide");
        deathMenuAnim.SetTrigger("Dead");
        buttonAnim.SetTrigger("Jump");

        //Check if this is highscore
        if (score > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", (int)Math.Ceiling(score));
        }
        
    }
}
