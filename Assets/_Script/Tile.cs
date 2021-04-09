using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int rowIdx = 0;
    public int colIdx = 0;



    //Comp
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //Check activated row and col and then set tile indication
        if (GridManager.instance.ActivatedRowIdx == rowIdx ||
            GridManager.instance.ActivatedColIdx == colIdx)
        {
            //Check if this tile is empty
            if (spriteRenderer.sprite != null)
            {
                //If buffer is empty
                if (GridManager.instance.CheckIfListOfBufferEmpty() == true)
                {
                    GridManager.instance.SelectTile(rowIdx, colIdx);
                    SetTileSprite(null);
                }
            }
        }

        //if(GlobalData.instance.GetGameOver() == true)
        //{
        //    return;
        //}

        ////If it's immovable sprite
        //if(GridManager.instance.immovableSprite == tileSprite)
        //{
        //    return;
        //}


        //// if we are swapping tile, return;
        //if(GridManager.instance.isProcessing == true)
        //{
        //    Debug.Log("Grid is swaping tile");
        //    return;
        //}


        ////If we don't have selected tile
        //if(GridManager.instance.selectedTile == null)
        //{
        //    GridManager.instance.selectedTile = this;
        //    SoundManager.instance.Play("SelectSFX");
        //    SelectTile();
        //}
        ////If it selected tile is me
        //else if(GridManager.instance.selectedTile == this)
        //{
        //    GridManager.instance.selectedTile = null;
        //    SoundManager.instance.Play("SelectSFX");
        //    DeselectTile();
        //}
        ////If select other tile
        //else
        //{
        //    //Check if we selected adjacent tile
        //    bool result = GridManager.instance.CheckAdjacentTile(this);
        //    if (result == true)
        //    {
        //        GridManager.instance.selectedTile.DeselectTile();
        //        SoundManager.instance.Play("SelectSFX");

        //        SoundManager.instance.Play("SwapSFX");
        //        GridManager.instance.SwapWithSelectedTile(this);
        //    }
        //    //if not
        //    else
        //    {
        //        GridManager.instance.selectedTile.DeselectTile();

        //        GridManager.instance.selectedTile = this;
        //        SoundManager.instance.Play("SelectSFX");
        //        SelectTile();
        //    }
        //    //GridManager.instance.selectedTile = this;
        //    //SelectTile();
        //}
    }

    private void OnMouseEnter()
    {
        //Check activated row and col and then set tile indication
        if (GridManager.instance.ActivatedRowIdx == rowIdx ||
            GridManager.instance.ActivatedColIdx == colIdx)
        {
            if (spriteRenderer.sprite != null)
            {
                GridManager.instance.SetSelectingTileIndication(rowIdx, colIdx);
            }
        }
    }

    //public void SelectTile()
    //{
    //    spriteRenderer.color = new Color(.5f, .5f, .5f, 1.0f);
    //}

    //public void DeselectTile()
    //{
    //    spriteRenderer.color = Color.white;
    //}

    public void SetTileIdx(int row, int col)
    {
        rowIdx = row;
        colIdx = col;
    }

    public void SetTileSprite(Sprite _tileSprite)
    {
        spriteRenderer.sprite = _tileSprite;
    }

    //public IEnumerator MoveToPosToMove()
    //{
    //    float time = 0;
    //    float tileSwapTime = GridManager.instance.tileSwapTime;
    //    Vector3 startPos = transform.position;


    //    while (time < tileSwapTime)
    //    {
    //        transform.position = Vector3.Lerp(startPos, posToMove, time / tileSwapTime);

    //        time += Time.deltaTime;
    //        yield return null;
    //    }

    //    transform.position = posToMove;

    //    //let gridmanager now this tile finished moving
    //    StartCoroutine(GridManager.instance.DecreaseMovingTile());

    //}
}

//public class Tile : MonoBehaviour
//{
//	[SerializeField] Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);

//	private static Tile previousSelected = null;

//	private SpriteRenderer render;
//	private bool isSelected = false;

//	//Will be used to get adjacent tile with direction
//	private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

//	public int rowIdx = 0;
//	public int colIdx = 0;

//	bool matchFound = false;

//	void Awake()
//	{
//		render = GetComponent<SpriteRenderer>();
//	}

//	private void Select()
//	{
//		isSelected = true;

//		render.color = selectedColor; // change to selected color
//		previousSelected = gameObject.GetComponent<Tile>();
//		//SFXManager.instance.PlaySFX(Clip.Select);
//	}

//	private void Deselect()
//	{
//		isSelected = false;
//		render.color = Color.white;
//		previousSelected = null;
//	}

//    private void OnMouseDown()
//    {
//		//Prevent player to select this tile
//		if (render.sprite == null || BoardManager.instance.IsShifting)
//		{
//			return;
//		}

//		//if it's already been selected, deselect
//		if (isSelected)
//		{ 
//			Deselect();
//		}
//		else
//		{
//			//Check if there's already another tile selected
//			if (previousSelected == null)
//			{ 
//				//Select current tile
//				Select();
//			}
//			else
//			{
//				//Check if we select adjacent tile
//				List<GameObject> adjacentTiles = GetAllAdjacentTiles();
//				if (adjacentTiles.Contains(previousSelected.gameObject))
//				{ 
//					//then swap
//					SwapTile(previousSelected.render);
//					previousSelected.ClearAllMatches();

//					previousSelected.Deselect();

//					ClearAllMatches();
//				}
//				//if not adjacent tile
//				else
//				{ 
//					previousSelected.Deselect();
//					Select();
//				}
//			}
//		}
//	}

//	public void SwapTile(SpriteRenderer render2)
//	{ 
//		//If we have same sprite, do nothing
//		if (render.sprite == render2.sprite)
//		{ 
//			return;
//		}

//		//Swap sprite
//		Sprite tempSprite = render2.sprite; 
//		render2.sprite = render.sprite;
//		render.sprite = tempSprite;
//		//SFXManager.instance.PlaySFX(Clip.Swap); // 6
//	}

//	private GameObject GetAdjacent(Vector2 castDir)
//	{
//		//Raycast hit it self!!!!! 
//		//Tunr off Edit -> Project Settings -> Physcis 2D -> Queries Start In Colliders to prevent self colliding
//		RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);
//		if (hit.collider != null)
//		{
//			Tile t = hit.collider.gameObject.GetComponent<Tile>();
//			return hit.collider.gameObject;
//		}
//		return null;
//	}

//	private List<GameObject> GetAllAdjacentTiles()
//	{
//		List<GameObject> adjacentTiles = new List<GameObject>();
//		for (int i = 0; i < adjacentDirections.Length; i++)
//		{
//			adjacentTiles.Add(GetAdjacent(adjacentDirections[i]));
//		}
//		return adjacentTiles;
//	}

//	private List<GameObject> FindMatch(Vector2 castDir)
//	{ 
//		List<GameObject> matchingTiles = new List<GameObject>();


//		RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);
//		//If we hit sth and that has the same sprite,
//		while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == render.sprite)
//		{
//			//Found same tile
//			matchingTiles.Add(hit.collider.gameObject);
//			hit = Physics2D.Raycast(hit.collider.transform.position, castDir);
//		}
//		return matchingTiles;
//	}

//	private void ClearMatch(Vector2[] paths)
//	{
//		List<GameObject> matchingTiles = new List<GameObject>(); 

//		for (int i = 0; i < paths.Length; i++)
//		{
//			matchingTiles.AddRange(FindMatch(paths[i]));
//		}

//		if (matchingTiles.Count >= 2)
//		{
//			for (int i = 0; i < matchingTiles.Count; i++)
//			{
//				matchingTiles[i].GetComponent<SpriteRenderer>().sprite = null;
//			}
//			matchFound = true; // 6
//		}
//	}

//	public void ClearAllMatches()
//	{
//		if (render.sprite == null)
//			return;

//		ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });
//		ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });

//		if (matchFound)
//		{
//			render.sprite = null;
//			matchFound = false;

//			StopCoroutine(BoardManager.instance.FindNullTiles());
//			StartCoroutine(BoardManager.instance.FindNullTiles());
//			//SFXManager.instance.PlaySFX(Clip.Clear);
//		}
//	}

//	public void SetIdx(int row, int col)
//    {
//		rowIdx = row;
//		colIdx = col;
//    }
//}
