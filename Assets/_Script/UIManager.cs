using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [Header("Timer")]
    [SerializeField] TMP_Text timerText;
    [SerializeField] float timer = 60.0f;

    [Header("GameOver")]
    [SerializeField] Canvas gameOverCanvas;

    [Header("SceneName")]
    [SerializeField] string mainMenuSceneName;
    [SerializeField] string gameSceneName;

    [Header("TextReference")]
    [SerializeField] TMP_Text difficultyText;
    [SerializeField] TMP_Text matchNumText;
    [SerializeField] TMP_Text gameOverTitleText;

    [SerializeField] GridManager gridManager;

    [Header("Score")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text finalScoreText;
    [SerializeField] TMP_Text moveCountText;
    [SerializeField] TMP_Text matchLeftText;

    protected override void Awake()
    {
        base.Awake();

        GlobalData.instance.OnScoreChanged.AddListener(OnScoreChangedCallback);
        GlobalData.instance.OnMoveCountChanged.AddListener(OnMoveCountChangedCallback);
        GlobalData.instance.OnMatchLeftChanged.AddListener(OnMatchLeftChangedCallback);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverCanvas.enabled = false;

        timerText.text = timer.ToString("F0");
        difficultyText.text = "Difficulty : " + GlobalData.instance.difficulty.ToString();
        matchNumText.text = "Match Num : " + ((int)(GlobalData.instance.difficulty)).ToString();
        moveCountText.text = "Move Count : " + GlobalData.instance.MoveCount.ToString();
        matchLeftText.text = "Match Left: " + GlobalData.instance.MatchLeft.ToString();

        //Star timer
        InvokeRepeating(nameof(Timer), 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Timer()
    {
        //if (GlobalData.instance.hasGameOver == false)
        //{
        //    timer -= Time.deltaTime;
        //    timerText.text = timer.ToString("F0");

        //    if (timer <= 0)
        //    {
        //        GlobalData.instance.hasGameOver = true;
        //        StartCoroutine(CheckGridProcess());
        //    }
        //}

        timer -= 1.0f;
        timerText.text = timer.ToString("F0");

        if (timer <= 0)
        {
            GameOver("Time is up, you lose");
        }
    }

    public void GameOver(string gameOverTitle)
    {
        //If we already game over, return;
        if(GlobalData.instance.GetGameOver() == true)
        {
            return;
        }

        CancelInvoke(nameof(Timer));
        GlobalData.instance.SetGameOver(true);
        gameOverTitleText.text = gameOverTitle; 

        StartCoroutine(CheckGridProcess());
    }

    //Keep checking until no match found or fisniehd moving tile
    IEnumerator CheckGridProcess()
    {
        while(true)
        {
            if(gridManager.isProcessing == false)
            {
                gameOverCanvas.enabled = true;
                finalScoreText.text = "Final Score : " + GlobalData.instance.Score.ToString();
                break;
            }

            yield return null;
        }
    }

    public void OnPlayAgain()
    {
        GlobalData.instance.ResetGlobalData();
        SceneManager.LoadScene(gameSceneName);
        
    }

    public void OnMainMenu()
    {
        GlobalData.instance.ResetGlobalData();
        SceneManager.LoadScene(mainMenuSceneName);
    }

    void OnScoreChangedCallback(int score)
    {
        scoreText.text = "Score : " + score.ToString();
    }

    void OnMoveCountChangedCallback(int moveCount)
    {
        moveCountText.text = "Move Count : " + moveCount.ToString();
    }

    void OnMatchLeftChangedCallback(int matchLeft)
    {
        matchLeftText.text = "Match Left: " + matchLeft.ToString();
    }
}
