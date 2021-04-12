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
    [SerializeField] TMP_Text skillLevelText;
    [SerializeField] TMP_Text gameOverTitleText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text finalScoreText;

    bool startTimer = false;

    protected override void Awake()
    {
        base.Awake();

        GlobalData.instance.OnScoreChanged.AddListener(OnScoreChangedCallback);
        GridManager.instance.OnTileClicked.AddListener(OnTileClickedCallback);
        GridManager.instance.OnAllAnswerSequenceCompleted.AddListener(OnAllAnswerSequenceCompletedCallback);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverCanvas.enabled = false;

        switch (GlobalData.instance.difficulty)
        {
            case GlobalData.EDifficulty.EASY:
                timer = 45.0f;
                break;

            case GlobalData.EDifficulty.MEDIUM:
                timer = 30.0f;
                break;

            case GlobalData.EDifficulty.HARD:
                timer = 15.0f;
                break;
        }

        switch (GlobalData.instance.skillLevel)
        {
            case GlobalData.ESkillLevel.Novice:
                timer += 5.0f;
                break;

            case GlobalData.ESkillLevel.Adept:
                timer += 10.0f;
                break;

            case GlobalData.ESkillLevel.Master:
                timer += 15.0f;
                break;
        }

        timerText.text = timer.ToString("F2");
        difficultyText.text = "Difficulty : " + GlobalData.instance.difficulty.ToString();
        skillLevelText.text = "Skill Level : " + GlobalData.instance.skillLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(startTimer)
        {
            Timer();
        }
    }

    void Timer()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 0.0f;
            timerText.text = timer.ToString("F2");
            GameOver("Time is up");
            
        }
        else
        {
            timerText.text = timer.ToString("F2");
        }
    }

    public void GameOver(string gameOverTitle)
    {
        //If we already game over, return;
        if(GlobalData.instance.GetGameOver() == true)
        {
            return;
        }

        //CancelInvoke(nameof(Timer));
        GlobalData.instance.SetGameOver(true);
        gameOverTitleText.text = gameOverTitle;
        startTimer = false;
        gameOverCanvas.enabled = true;

        finalScoreText.text = "Final Score : " + (GlobalData.instance.Score + timer).ToString("F02");
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

    void OnTileClickedCallback()
    {
        GridManager.instance.OnTileClicked.RemoveListener(OnTileClickedCallback);

        //When tile clicked, start timer
        //InvokeRepeating(nameof(Timer), 1.0f, 1.0f);
        startTimer = true;
    }

    void OnAllAnswerSequenceCompletedCallback()
    {
        GameOver("All sequence completed");
    }

    void OnScoreChangedCallback(int score)
    {
        scoreText.text = "Score : " + score.ToString();
    }
}
