using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Comp
    SpriteRenderer spriteRendrer;



    protected override void Awake()
    {
        base.Awake();

        spriteRendrer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //switch (GlobalData.instance.difficulty)
        //{
        //    case GlobalData.EDifficulty.EASY:
        //        matchNum = (int)GlobalData.EDifficulty.EASY;
        //        break;

        //    case GlobalData.EDifficulty.MEDIUM:
        //        matchNum = (int)GlobalData.EDifficulty.MEDIUM;
        //        break;

        //    case GlobalData.EDifficulty.HARD:
        //        matchNum = (int)GlobalData.EDifficulty.HARD;
        //        break;
        //}


        GenerateGrid();
        GenerateSelectingIndication();

        //Active last row
        activatedRowIdx = gridRow - 1;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        tileSize.x = gridSize.x / (float)gridCol; //Get tile's width based on space and with of grid
        tileSize.y = gridSize.y / (float)gridRow; //Get tile's height based on space and with of grid

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
                GameObject tileGameObject = Instantiate(tilePrefab, transform);

                //Save it to grid as tile
                Tile tileComp = tileGameObject.GetComponent<Tile>();
                grid[row, col] = tileComp;

                //Set tile idx
                tileComp.SetTileIdx(row, col);

                //Set the size of tile
                SpriteRenderer tileRenderer = tileGameObject.GetComponent<SpriteRenderer>();
                tileRenderer.size = tileSize;

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
                //int randomIdx = Random.Range(0, possibleTileSprite.Count);
                //tileComp.SetTileSprite(possibleTileSprite[randomIdx]);
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
        selectingIndicationRow = Instantiate(selectingIndicationPrefab, transform);

        //Set the scale of inidcation
        Vector3 scale = new Vector3(gridRow, 1, 1);
        selectingIndicationRow.transform.localScale = scale;


        //Set the position of tile
        Vector3 pos = Vector3.zero;

        pos.x = transform.position.x;

        Vector3 lastRowPos = GetTilePos(gridRow - 1, 0);
        pos.y = lastRowPos.y;
        pos.z = -1.0f;

        selectingIndicationRow.transform.position = pos;




        //create
        selectingIndicationCol = Instantiate(selectingIndicationPrefab, transform);

        //Set the scale of inidcation
        scale = new Vector3(1, gridCol, 1);
        selectingIndicationCol.transform.localScale = scale;


        //Set the position of tile
        pos = Vector3.zero;

        Vector3 firstColPos = GetTilePos(0, 0);
        pos.x = firstColPos.x;
        pos.y = transform.position.y;
        pos.z = -1.0f;

        selectingIndicationCol.transform.position = pos;
        selectingIndicationCol.SetActive(false);


        //create
        selectingTileIndication = Instantiate(selectingTileIndicationPrefab, transform);

        //Set the position of tile
        pos.x = transform.position.x;
        pos.y = transform.position.y;
        pos.z = -3.0f;

        selectingTileIndication.transform.position = pos;
        selectingTileIndication.SetActive(false);
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

    //public void SwapWithSelectedTile(Tile currentTile)
    //{
    //    isProcessing = true;

    //    //Start moving tile
    //    StartCoroutine(SwapTile(currentTile));
    //}

    //IEnumerator SwapTile(Tile currentTile)
    //{
    //    float time = 0;
    //    Vector3 currentTilePos = currentTile.transform.position;
    //    Vector3 selectedTilePos = selectedTile.transform.position;

    //    while (time < tileSwapTime)
    //    {
    //        currentTile.transform.position = Vector3.Lerp(currentTilePos, selectedTilePos, time / tileSwapTime);
    //        selectedTile.transform.position = Vector3.Lerp(selectedTilePos, currentTilePos, time / tileSwapTime);

    //        time += Time.deltaTime;
    //        yield return null;
    //    }

    //    //finish moving
    //    currentTile.transform.position = selectedTilePos;
    //    selectedTile.transform.position = currentTilePos;

    //    //Swap tile in grid
    //    grid[currentTile.rowIdx, currentTile.colIdx] = selectedTile;
    //    grid[selectedTile.rowIdx, selectedTile.colIdx] = currentTile;

    //    //swap tile's idx
    //    int curTileRowIdx = currentTile.rowIdx;
    //    int curTileColIdx = currentTile.colIdx;

    //    currentTile.SetTileIdx(selectedTile.rowIdx, selectedTile.colIdx);
    //    selectedTile.SetTileIdx(curTileRowIdx, curTileColIdx);




    //    //Find match
    //    List<Tile> curTileMatch = FindMatch(currentTile);
    //    List<Tile> selTileMatch = FindMatch(selectedTile);

    //    //Decrease move count
    //    GlobalData.instance.ModifyMoveCount(-1);
    //    if (GlobalData.instance.MoveCount <= 0)
    //    {
    //        UIManager.instance.GameOver("Used all move count, you lose");
    //    }

    //    if (curTileMatch.Count != 0 || selTileMatch.Count != 0)
    //    {
    //        SoundManager.instance.Play("MatchClearSFX");
    //        //Wait a bit so that user can see match
    //        yield return new WaitForSeconds(matchPopWaitTime);
    //    }

    //    //Set all mathced tile to null
    //    foreach(Tile tile in curTileMatch)
    //    {
    //        tile.SetTileSprite(null);
    //    }
    //    foreach (Tile tile in selTileMatch)
    //    {
    //        tile.SetTileSprite(null);
    //    }

    //    //Add score
    //    GlobalData.instance.ModifyScore((curTileMatch.Count + selTileMatch.Count )* tileScore);

    //    //Reset selected tile
    //    selectedTile = null;


    //    //If there was no matched tile.. finish processing
    //    if (curTileMatch.Count == 0 && selTileMatch.Count == 0)
    //    {
    //        isProcessing = false;
    //    }
    //    //If there was matched tile, refill
    //    else
    //    {
    //        //Refill tiles
    //        RefillTiles();
    //    }
    //}

    //public bool CheckAdjacentTile(Tile currentTile)
    //{
    //    int curTileRowIdx = currentTile.rowIdx;
    //    int curTileColIdx = currentTile.colIdx;
    //    int selTileRowIdx = selectedTile.rowIdx;
    //    int selTileColIdx = selectedTile.colIdx;

    //    //Check if left
    //    if(selTileColIdx - 1 == curTileColIdx &&
    //        selTileRowIdx == curTileRowIdx)
    //    {
    //        return true;
    //    }
    //    //Check if right
    //    else if (selTileColIdx + 1 == curTileColIdx &&
    //        selTileRowIdx == curTileRowIdx)
    //    {
    //        return true;
    //    }
    //    //Check if top
    //    if (selTileColIdx == curTileColIdx &&
    //        selTileRowIdx + 1 == curTileRowIdx)
    //    {
    //        return true;
    //    }

    //    //Check if bottom
    //    if (selTileColIdx == curTileColIdx &&
    //        selTileRowIdx - 1 == curTileRowIdx)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //List<Sprite> CheckIdenticalSprite(Tile tile)
    //{
    //    List<Sprite> identicalSprite = new List<Sprite>();

    //    //check col first
    //    if (tile.colIdx >= (matchNum - 1))
    //    {
    //        //Get left tile sprite
    //        Sprite checkingSprite = grid[tile.rowIdx, tile.colIdx - 1].tileSprite;

    //        //Start checking identical sprite
    //        int identicalNum = 0;
    //        for (int col = tile.colIdx - 2; col >= tile.colIdx - (matchNum - 1); --col)
    //        {
    //            //If has same sprite, count
    //            if (grid[tile.rowIdx, col].tileSprite == checkingSprite)
    //            {
    //                ++identicalNum;
    //            }
    //            //if it's diffenrt sprite, just quick chekcing
    //            else
    //            {
    //                break;
    //            }
    //        }

    //        //If there is identical col, add it to identical sprite
    //        if (identicalNum == (matchNum - 2))
    //        {
    //            identicalSprite.Add(checkingSprite);
    //        }
    //    }

    //    //check bottom
    //    if (tile.rowIdx >= (matchNum - 1))
    //    {
    //        //Get bottom tile sprite
    //        Sprite checkingSprite = grid[tile.rowIdx - 1, tile.colIdx].tileSprite;

    //        //Start checking identical sprite
    //        int identicalNum = 0;
    //        for(int row = tile.rowIdx - 2; row >= tile.rowIdx - (matchNum - 1); --row)
    //        {
    //            //If has same sprite, count
    //            if(grid[row, tile.colIdx].tileSprite == checkingSprite)
    //            {
    //                ++identicalNum;
    //            }
    //            //if it's diffenrt sprite, just quick chekcing
    //            else
    //            {
    //                break;
    //            }
    //        }

    //        //If there is identical col, add it to identical sprite
    //        if(identicalNum == (matchNum - 2))
    //        {
    //            identicalSprite.Add(checkingSprite);
    //        }
    //    }

    //    return identicalSprite;
    //}

    //List<Tile> FindMatch(Tile tile)
    //{
    //    Sprite checkingSprite = tile.tileSprite;

    //    int numOfSameTile = 0;
    //    List<Tile> listOfSameTileInRow = new List<Tile>();

    //    //Check row
    //    for(int col = 0; col < gridCol; ++col)
    //    {
    //        //If this tile have the same sprite..
    //        if(grid[tile.rowIdx, col].tileSprite == checkingSprite)
    //        {
    //            ++numOfSameTile;
    //            listOfSameTileInRow.Add(grid[tile.rowIdx, col]);
    //        }
    //        //If this tile has diffenrt tile and if we already started counting
    //        else if(grid[tile.rowIdx, col].tileSprite != checkingSprite &&
    //            numOfSameTile != 0)
    //        {
    //            //we found the match
    //            if (numOfSameTile >= matchNum)
    //            {
    //                break;
    //            }
    //            //Start counting again
    //            else
    //            {
    //                numOfSameTile = 0;
    //                listOfSameTileInRow.Clear();
    //            }
    //        }
    //    }

    //    //If the number of identical tile is less than match num, clear
    //    if(listOfSameTileInRow.Count < matchNum)
    //    {
    //        listOfSameTileInRow.Clear();
    //    }
    //    //If we found match
    //    else
    //    {
    //        GlobalData.instance.ModifyMatchLeft(-1);
    //    }


    //    numOfSameTile = 0;
    //    List<Tile> listOfSameTileInCol = new List<Tile>();

    //    //Check col
    //    for (int row = 0; row < gridRow; ++row)
    //    {
    //        //If this tile have the same sprite..
    //        if (grid[row, tile.colIdx].tileSprite == checkingSprite)
    //        {
    //            ++numOfSameTile;
    //            listOfSameTileInCol.Add(grid[row, tile.colIdx]);
    //        }
    //        //If this tile has diffenrt tile and if we already started counting
    //        else if (grid[row, tile.colIdx].tileSprite != checkingSprite &&
    //            numOfSameTile != 0)
    //        {
    //            //we found the match
    //            if (numOfSameTile >= matchNum)
    //            {
    //                break;
    //            }
    //            //Start counting again
    //            else
    //            {
    //                numOfSameTile = 0;
    //                listOfSameTileInCol.Clear();
    //            }
    //        }
    //    }

    //    //If the number of identical tile is less than match num, clear
    //    if (listOfSameTileInCol.Count < matchNum)
    //    {
    //        listOfSameTileInCol.Clear();
    //    }
    //    else
    //    {
    //        GlobalData.instance.ModifyMatchLeft(-1);
    //    }

    //    //CHeck match left
    //    if (GlobalData.instance.MatchLeft <= 0)
    //    {
    //        UIManager.instance.GameOver("All match found, you win");
    //    }

    //    //************THis will contain duplicated center tile**********************
    //    //if you don't want, need to delete duplicated one
    //    listOfSameTileInRow.AddRange(listOfSameTileInCol);
    //    return listOfSameTileInRow;
    //}

    //void RefillTiles()
    //{
    //    //Find all null tile distinguised by col
    //    //Col, list of null tile
    //    Dictionary<int, List<Tile>> nullTileList = new Dictionary<int, List<Tile>>();

    //    bool isFirstNullCol = false;
    //    for (int col = 0; col < gridCol; ++col)
    //    {
    //        //Find any null tile in one col
    //        List<Tile> colNullTile = new List<Tile>();
    //        for (int row = 0; row < gridRow; ++row)
    //        {
    //            //Find null tile
    //            if(grid[row, col].tileSprite == null)
    //            {
    //                colNullTile.Add(grid[row, col]);
    //            }
    //        }

    //        //Add this col null tile to dictionary
    //        if (colNullTile.Count != 0)
    //        {
    //            nullTileList.Add(col, colNullTile);

    //            //Save first col of null tile
    //            if(isFirstNullCol == false)
    //            {
    //                startColIdxOfMovingTiles = col;
    //                isFirstNullCol = true;
    //            }

    //            //Save last col
    //            lastColIdxOfMovingTiles = col;
    //        }
    //    }

    //    //List of tiles that need to move
    //    List<Tile> movingTiles = new List<Tile>();

    //    foreach(KeyValuePair<int, List<Tile>> element in nullTileList)
    //    {
    //        int nullTilesCol = element.Key;
    //        List<Tile> nullTiles = element.Value;
    //        movingTiles.AddRange(nullTiles); //add all null tile to moving tile

    //        //how many tile should null tile move
    //        int lastNullTileRowIdx = nullTiles[nullTiles.Count - 1].rowIdx;
    //        int numToMoveUp = gridRow - nullTiles[0].rowIdx;
    //        int numToMoveDown = nullTiles.Count;

    //        //Move all null tile to up and set new row idx and position to move
    //        foreach(Tile nullTile in nullTiles)
    //        {
    //            nullTile.transform.position = GetTilePos(nullTile.rowIdx + numToMoveUp, nullTilesCol);
    //            nullTile.rowIdx = nullTile.rowIdx + numToMoveUp - numToMoveDown; //set new row idx
    //            nullTile.posToMove = GetTilePos(nullTile.rowIdx, nullTile.colIdx); //Set next position to move
    //            nullTile.SetTileSprite(tileSprites[Random.Range(0, tileSprites.Count)]);
    //            SpriteRenderer tileRenderer = nullTile.GetComponent<SpriteRenderer>();
    //            tileRenderer.size = tileSize;
    //        }

    //        //Find all not null tile above last null tile and set new row idx and position to move
    //        for(int row = lastNullTileRowIdx + 1; row < gridRow; ++row)
    //        {
    //            grid[row, nullTilesCol].rowIdx = grid[row, nullTilesCol].rowIdx - numToMoveDown;
    //            grid[row, nullTilesCol].posToMove = GetTilePos(grid[row, nullTilesCol].rowIdx, grid[row, nullTilesCol].colIdx);
    //            movingTiles.Add(grid[row, nullTilesCol]);
    //        }
    //    }

    //    //CHange grid's tile to all moving tile
    //    foreach(Tile movingTile in movingTiles)
    //    {
    //        //Set all moving tile to grid
    //        grid[movingTile.rowIdx, movingTile.colIdx] = movingTile;

    //        //Make it move
    //        StartCoroutine(movingTile.MoveToPosToMove());
    //    }

    //    numOfMovingTile = movingTiles.Count;
    //}

    //Vector3 GetTilePos(int row, int col)
    //{
    //    Vector3 tilePos = new Vector3(tileStartPos.x + (col * tileSize.x),
    //                                  tileStartPos.y + (row * tileSize.y),
    //                                  -1.0f);

    //    return tilePos;
    //}

    ////This will be called by moving tile when they finished moving
    //public IEnumerator DecreaseMovingTile()
    //{
    //    --numOfMovingTile;

    //    //if all moving tile finished moving..
    //    if(numOfMovingTile == 0)
    //    {
    //        //Wait a bit so that user can see match
    //        yield return new WaitForSeconds(matchPopWaitTime);

    //        bool hasFoundMatch = false;
    //        //Find match for all moved tile
    //        for (int row = 0; row < gridRow; ++row)
    //        {
    //            //Check only moved tiles col
    //            for (int col = startColIdxOfMovingTiles; col <= lastColIdxOfMovingTiles; ++col)
    //            {
    //                List<Tile> tileMatch = FindMatch(grid[row, col]);

    //                //If found match
    //                if (tileMatch.Count > 0)
    //                {
    //                    hasFoundMatch = true;
    //                    SoundManager.instance.Play("MatchClearSFX");

    //                    //Set all mathced tile to null
    //                    foreach (Tile tile in tileMatch)
    //                    {
    //                        tile.SetTileSprite(null);
    //                    }

    //                    //Add score
    //                    GlobalData.instance.ModifyScore(tileMatch.Count * tileScore);

    //                    //Only find 1 match
    //                    break;
    //                }
    //            }

    //            //Only find 1 match
    //            if(hasFoundMatch)
    //            {
    //                break;
    //            }
    //        }

    //        //If there was no matched tile.. finish processing
    //        if (hasFoundMatch == false)
    //        {
    //            isProcessing = false;
    //        }
    //        //If there was matched tile, refill
    //        else
    //        {
    //            //Refill tiles
    //            RefillTiles();
    //        }
    //    }
    //}
}