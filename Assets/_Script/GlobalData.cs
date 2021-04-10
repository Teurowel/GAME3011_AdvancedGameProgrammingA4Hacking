using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalData : Singleton<GlobalData>
{
    bool hasGameOver = false;

    public EDifficulty difficulty = EDifficulty.EASY;
    public ESkillLevel skillLevel = ESkillLevel.Novice;

    [HideInInspector]
    public UnityEvent<int> OnScoreChanged; //UIManager subscibe this

    public int Score => score;
    int score = 0;


    //Match number
    public enum EDifficulty
    {
        EASY,
        MEDIUM,
        HARD
    }

    public enum ESkillLevel
    {
        Novice,
        Adept,
        Master
    }

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetGameOver()
    {
        return hasGameOver;
    }

    public void SetGameOver(bool gameOver)
    {
        hasGameOver = gameOver;
    }

    public void ModifyScore(int modifier)
    {
        score += modifier;

        if(OnScoreChanged != null)
        {
            OnScoreChanged.Invoke(score);
        }
    }

    public void ResetGlobalData()
    {
        hasGameOver = false;
        score = 0;
    }
}
