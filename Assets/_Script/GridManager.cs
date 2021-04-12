using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GridManager : Singleton<GridManager>
{
    [Header("Grid Setting")]
    [SerializeField] int gridCol = 10;
    [SerializeField] int gridRow = 10;
    [SerializeField] float gridMarginTop = 0;
    [SerializeField] float gridMarginBottom = 0;
    [SerializeField] float gridMarginLeft = 0;
    [SerializeField] float gridMarginRight = 0;
    [SerializeField] GameObject selectingIndicationPrefab;
    [SerializeField] GameObject selectingTileIndicationPrefab;

    [Header("Tile Setting")]
    [SerializeField] GameObject tilePrefab;
    [SerializeField] List<Sprite> tileSprites;

    [Header("Buffer Setting")]
    [SerializeField] GameObject bufferGameObject;
    [SerializeField] GameObject bufferTilePrefab;
    [SerializeField] GameObject bufferTileIndicationPrefab;
    [SerializeField] int bufferCount = 4;

    [Header("Answer Sequence Setting")]
    [SerializeField] List<AnswerSequence> listOfAnswerSequence;
    [SerializeField] int answerSequenceCount = 1;
    [SerializeField] int sequenceTileMinSize = 2;
    [SerializeField] int sequenceTileMaxSize = 4;

    Tile[,] grid;

    public Tile selectedTile = null;
    public bool isProcessing = false; //will be true when swapping tiles, moving tiles, checking match etc...

    Vector2 gridSize = Vector2.zero;

    Vector2 tileStartPos = Vector2.zero; //position where 0,0 tile start
    Vector2 tileSize = Vector2.zero; //size of one tile


    GameObject selectingIndicationRow; //row inidixation
    GameObject selectingIndicationCol; //Col inidixation
    GameObject selectingTileIndication;

    public int ActivatedRowIdx => activatedRowIdx;
    
    int activatedRowIdx = -1; //which row can be selcted?
    public int ActivatedColIdx => activatedColIdx;
    int activatedColIdx = -1; //which row can be selcted?

    public int NumOfTileType => numOfTileType;
    int numOfTileType = 0; //this will be different by difficulty

    /////////////////////////////Buffer////////////////////////////
    public List<GameObject> ListOfBuffer => listOfBuffer;
    List<GameObject> listOfBuffer = new List<GameObject>();
    int bufferEmptyIdx = 0;
    public Vector3 BufferTileSize => bufferTileSize;
    Vector3 bufferTileSize = Vector3.zero;
    Vector3 bufferTileStartPos = Vector3.zero;
    ////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////Sequence////////////////////////////
    int numOfCompletedAnswerSequence = 0;

    ////////////////////////////////////////////////////////////////////////////////////
    //Comp
    SpriteRenderer spriteRendrer;


    [HideInInspector]
    public UnityEvent OnTileClicked; //UIManager's timer will subscribte this to start timber
    [HideInInspector]
    public UnityEvent OnAllAnswerSequenceCompleted;// UIManager will subscribe this

    protected override void Awake()
    {
        base.Awake();

        spriteRendrer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (GlobalData.instance.difficulty)
        {
            case GlobalData.EDifficulty.EASY:
                gridCol = 5;
                gridRow = 5;
                numOfTileType = 4;
                bufferCount = 4;
                answerSequenceCount = 1;
                sequenceTileMinSize = 2;
                sequenceTileMaxSize = 4;
                break;

            case GlobalData.EDifficulty.MEDIUM:
                gridCol = 6;
                gridRow = 6;
                numOfTileType = 5;
                bufferCount = 6;
                answerSequenceCount = 2;
                sequenceTileMinSize = 2;
                sequenceTileMaxSize = 4;
                break;

            case GlobalData.EDifficulty.HARD:
                gridCol = 6;
                gridRow = 6;
                numOfTileType = 5;
                bufferCount = 6;
                answerSequenceCount = 3;
                sequenceTileMinSize = 2;
                sequenceTileMaxSize = 4;
                break;
        }


        GenerateGrid();
        GenerateSelectingIndication();
        GenerateBuffer();
        GenerateAnswerSequence();

        //Active last row
        activatedRowIdx = gridRow - 1;
    }


    void GenerateGrid()
    {
        int numOfColSpace = gridCol - 1; //get number of col space between col tiles
        int numOfRowSpace = gridRow - 1; //get number of row space between col tiles

        //Get grid size
        gridSize = spriteRendrer.bounds.size;
        gridSize.x -= gridMarginLeft + gridMarginRight;
        gridSize.y -= gridMarginTop + gridMarginBottom;
        //Debug.Log(gridSize);

        //Calculate tile size
        tileSize = Vector2.zero;
        tileSize.x = transform.lossyScale.x / (float)gridCol; //Get tile's width based on space and with of grid
        tileSize.y = transform.lossyScale.y / (float)gridRow; //Get tile's height based on space and with of grid


        //Calcualte tile's start position
        tileStartPos.x = transform.position.x + -(gridSize.x * 0.5f) + (tileSize.x * 0.5f);
        tileStartPos.y = transform.position.y + -(gridSize.y * 0.5f) + (tileSize.y * 0.5f);

        //create new grid
        grid = new Tile[gridRow, gridCol];

        //nested for loop to generate tiles to fill gird
        for (int row = 0; row < gridRow; ++row)
        {
            for (int col = 0; col < gridCol; ++col)
            {
                //create one tile
                GameObject tileGameObject = Instantiate(tilePrefab);

                //Save it to grid as tile
                Tile tileComp = tileGameObject.GetComponent<Tile>();
                grid[row, col] = tileComp;

                //Set tile idx
                tileComp.SetTileIdx(row, col);

                //Set the size of tile
                //SpriteRenderer tileRenderer = tileGameObject.GetComponent<SpriteRenderer>();
                //tileRenderer.size = tileSize;
                tileGameObject.transform.localScale = tileSize;
                tileGameObject.transform.SetParent(transform);

                //////////////////////////Set random sprite//////////////////////////
                //List<Sprite> possibleTileSprite = new List<Sprite>(); //make candidates for random sprite
                //possibleTileSprite.AddRange(tileSprites);

                ////Check identical sprite
                //List<Sprite> identicalSprite = CheckIdenticalSprite(tileComp);
                //foreach(Sprite identical in identicalSprite)
                //{
                //    possibleTileSprite.Remove(identical);
                //}

                ////pick random sprite from possible sprites
                int randomIdx = Random.Range(0, numOfTileType);
                tileComp.SetTileSprite(tileSprites[randomIdx]);
                //////////////////////////////////////////////////////////////////////////////

                //Set the position of tile
                Vector3 tilePos = Vector3.zero;

                tilePos.x = tileStartPos.x + (col * tileSize.x);
                tilePos.y = tileStartPos.y + (row * tileSize.y);
                tilePos.z = -2.0f;

                tileGameObject.transform.position = tilePos;
            }
        }
    }

    void GenerateSelectingIndication()
    {
        //create
        selectingIndicationRow = Instantiate(selectingIndicationPrefab);

        //Set the scale of inidcation
        Vector3 scale = new Vector3(transform.lossyScale.x, tileSize.y, 1);
        selectingIndicationRow.transform.localScale = scale;
        selectingIndicationRow.transform.SetParent(transform);


        //Set the position of tile
        Vector3 pos = Vector3.zero;

        pos.x = transform.position.x;

        Vector3 lastRowPos = GetTilePos(gridRow - 1, 0);
        pos.y = lastRowPos.y;
        pos.z = -1.0f;

        selectingIndicationRow.transform.position = pos;




        //create
        selectingIndicationCol = Instantiate(selectingIndicationPrefab);

        //Set the scale of inidcation
        scale = new Vector3(tileSize.x, transform.lossyScale.y, 1);
        selectingIndicationCol.transform.localScale = scale;
        selectingIndicationCol.transform.SetParent(transform);

        //Set the position of tile
        pos = Vector3.zero;

        Vector3 firstColPos = GetTilePos(0, 0);
        pos.x = firstColPos.x;
        pos.y = transform.position.y;
        pos.z = -1.0f;

        selectingIndicationCol.transform.position = pos;
        selectingIndicationCol.SetActive(false);


        //create
        selectingTileIndication = Instantiate(selectingTileIndicationPrefab);

        selectingTileIndication.transform.localScale = tileSize;
        selectingTileIndication.transform.SetParent(transform);

        //Set the position of tile
        pos.x = transform.position.x;
        pos.y = transform.position.y;
        pos.z = -3.0f;

        selectingTileIndication.transform.position = pos;
        selectingTileIndication.SetActive(false);
    }

    void GenerateBuffer()
    {
        //Get grid size
        Vector3 bufferGridSize = bufferGameObject.GetComponent<SpriteRenderer>().bounds.size;
        //Debug.Log(gridSize);

        //Calculate tile size
        bufferTileSize = Vector2.zero;
        bufferTileSize.x = bufferGameObject.transform.lossyScale.x / (float)bufferCount; //Get tile's width based on space and with of grid
        bufferTileSize.y = bufferGameObject.transform.lossyScale.y;

        //Calcualte tile's start position
        bufferTileStartPos = Vector3.zero;
        bufferTileStartPos.x = bufferGameObject.transform.position.x + -(bufferGridSize.x * 0.5f) + (bufferTileSize.x * 0.5f);
        bufferTileStartPos.y = bufferGameObject.transform.position.y;

        for (int col = 0; col < bufferCount; ++col)
        {
            //create one tile
            GameObject tileGameObject = Instantiate(bufferTilePrefab);
            listOfBuffer.Add(tileGameObject);

            //Set the size of tile
            tileGameObject.transform.localScale = bufferTileSize;
            tileGameObject.transform.SetParent(bufferGameObject.transform);

            //Set the position of tile
            Vector3 tilePos = Vector3.zero;

            tilePos.x = bufferTileStartPos.x + (col * bufferTileSize.x);
            tilePos.y = bufferTileStartPos.y;
            tilePos.z = -2.0f;

            tileGameObject.transform.position = tilePos;



            //create buffre tile indication
            GameObject tileIndication = Instantiate(bufferTileIndicationPrefab);

            //Set the size of tile
            tileIndication.transform.localScale = bufferTileSize;
            tileIndication.transform.SetParent(bufferGameObject.transform);
            tileIndication.transform.position = tilePos;
        }
        
    }

    void GenerateAnswerSequence()
    {
        for(int i =0; i < answerSequenceCount; ++i)
        {
            listOfAnswerSequence[i].GenerateSequence(Random.Range(sequenceTileMinSize, sequenceTileMaxSize + 1), tileSprites, bufferTileSize, bufferTileStartPos);
        }
    }

    Vector3 GetTilePos(int row, int col)
    {
        Vector3 tilePos = new Vector3(tileStartPos.x + (col * tileSize.x),
                                      tileStartPos.y + (row * tileSize.y),
                                      -1.0f);

        return tilePos;
    }

    //Set position and activate selcting tile indication
    public void SetSelectingTileIndication(int row, int col)
    {
        selectingTileIndication.SetActive(true);

        Vector3 pos = GetTilePos(row, col);
        pos.z = -3.0f;
        selectingTileIndication.transform.position = pos;
    }

    public void SetSelectingIndicationRow(int row)
    {
        //Deactivated col
        activatedColIdx = -1;
        selectingIndicationCol.SetActive(false);

        //Set the position of tile
        Vector3 pos = Vector3.zero;

        pos.x = transform.position.x;

        Vector3 rowPos = GetTilePos(row, 0);
        pos.y = rowPos.y;
        pos.z = -1.0f;

        selectingIndicationRow.transform.position = pos;

        selectingIndicationRow.SetActive(true);

        activatedRowIdx = row;
    }

    public void SetSelectingIndicationCol(int col)
    {
        //Deactivate row
        activatedRowIdx = -1;
        selectingIndicationRow.SetActive(false);

        //Set the position of tile
        Vector3 pos = Vector3.zero;

        Vector3 colPos = GetTilePos(0, col);
        pos.x = colPos.x;
        pos.y = transform.position.y;
        pos.z = -1.0f;

        selectingIndicationCol.transform.position = pos;

        selectingIndicationCol.SetActive(true);

        activatedColIdx = col;
    }

    public void SelectTile(int row, int col)
    {
        if(OnTileClicked != null)
        {
            OnTileClicked.Invoke();
        }

        //add selected tile to buffer
        listOfBuffer[bufferEmptyIdx].GetComponent<SpriteRenderer>().sprite = grid[row, col].SpriteRenderer.sprite;

        //sound
        SoundManager.instance.Play("SelectSFX");


        //change slecting indication row and col
        //if row was acitavted
        if (activatedRowIdx != -1)
        {
            SetSelectingIndicationCol(col);
        }
        //If col was activated
        else
        {
            SetSelectingIndicationRow(row);
        }


        //CHeck answer sequence
        for (int i = 0; i < answerSequenceCount; ++i)
        {
            listOfAnswerSequence[i].CheckSequence(grid[row, col].SpriteRenderer.sprite, bufferEmptyIdx);
        }

        ++bufferEmptyIdx;
    }

    public bool CheckIfListOfBufferEmpty()
    {
        if (bufferEmptyIdx == listOfBuffer.Count)
        {
            return false;
        }

        return true;
    }

    public void IncreaseNumOfCompletedAnswerSequence()
    {
        ++numOfCompletedAnswerSequence;

        if(numOfCompletedAnswerSequence == answerSequenceCount)
        {
            if(OnAllAnswerSequenceCompleted != null)
            {
                OnAllAnswerSequenceCompleted.Invoke();
            }
        }
    }
}
