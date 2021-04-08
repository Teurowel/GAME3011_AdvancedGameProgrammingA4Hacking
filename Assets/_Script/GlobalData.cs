using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalData : Singleton<GlobalData>
{
    bool hasGameOver = false;

    public EDifficulty difficulty = EDifficulty.EASY;

    [HideInInspector]
    public UnityEvent<int> OnScoreChanged; //UIManager subscibe this

    public int Score => score;
    int score = 0;


    [HideInInspector]
    public UnityEvent<int> OnMoveCountChanged; //UIManager subscribe this

    public int MoveCount => moveCount;
    [SerializeField] int moveCount = 30;
    int originMoveCount = 0; //When reset game, moveCount will be set to this

    [HideInInspector]
    public UnityEvent<int> OnMatchLeftChanged; //UIManager subscribe this

    public int MatchLeft => matchLeft;
    [SerializeField] int matchLeft = 10;
    int originMatchLeft = 0; //When reset game, match left will be set to this

    //Match number
    public enum EDifficulty
    {
        EASY = 3,
        MEDIUM = 4,
        HARD = 5
    }

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);

        originMoveCount = moveCount;
        originMatchLeft = matchLeft;
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

    public void ModifyMoveCount(int modifier)
    {
        moveCount += modifier;

        if (OnMoveCountChanged != null)
        {
            OnMoveCountChanged.Invoke(moveCount);
        }
    }

    public void ModifyMatchLeft(int modifier)
    {
        matchLeft += modifier;

        if(matchLeft <= 0)
        {
            matchLeft = 0;
        }

        if (OnMatchLeftChanged != null)
        {
            OnMatchLeftChanged.Invoke(matchLeft);
        }
    }

    public void ResetGlobalData()
    {
        hasGameOver = false;
        score = 0;
        moveCount = originMoveCount;
        matchLeft = originMatchLeft;
    }
}
