//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BoardManager : Singleton<BoardManager>
//{
//	[Header("Grid Setting")]
//	[SerializeField] int gridRow;
//	[SerializeField] int gridCol;

//	[Header("Tile Setting")]
//	[SerializeField] GameObject tilePrefab;
//	[SerializeField] List<Sprite> tileSprites = new List<Sprite>(); //list of sprites that you¡¯ll use as your tile pieces.


//	private GameObject[,] tiles;

//	public bool IsShifting { get; set; }

//	void Start()
//	{
//		//if(Application.IsPlaying(gameObject))
//		//      {
//		//	//Play logic
//		//	//GEt tile size
//		//	Vector2 tileSize = tilePrefab.GetComponent<SpriteRenderer>().bounds.size;

//		//	//Create grid
//		//	CreateGrid(tileSize);
//		//}
//		//else
//		//      {
//		//	//Editor logic
//		//	//GEt tile size
//		//	Vector2 tileSize = tilePrefab.GetComponent<SpriteRenderer>().bounds.size;

//		//	//Create grid
//		//	CreateGrid(tileSize);
//		//}

//		Vector2 tileSize = tilePrefab.GetComponent<SpriteRenderer>().bounds.size;

//		//Create grid
//		CreateGrid(tileSize);
//	}

//	private void CreateGrid(Vector2 tileSize)
//	{
//		tiles = new GameObject[gridRow, gridCol];

//		float startX = transform.position.x;
//		float startY = transform.position.y;

//		Sprite[] previousLeftCol = new Sprite[gridRow]; //Store all sprite in left col from current tile
//		Sprite previousBelow = null; //Store sprite below current tile

//		for (int col = 0; col < gridCol; col++)
//		{
//			for (int row = 0; row < gridRow; row++)
//			{
//				//Create tile
//				GameObject newTile = Instantiate(tilePrefab,
//				new Vector3(startX + (tileSize.x * col),
//							startY + (tileSize.y * row), 0), tilePrefab.transform.rotation);

//				//Make new tile's parent to board
//				newTile.transform.parent = transform;

//				//Save tile to array
//				tiles[col, row] = newTile;

//				/////////////////////Prevent row or col of 3identical sprite ////////////////
//				List<Sprite> possibleSprite = new List<Sprite>(); //Make possible sprite list
//				possibleSprite.AddRange(tileSprites); //copy all tile sprite first

//				possibleSprite.Remove(previousLeftCol[row]); //Remove left sprite from current tile
//				possibleSprite.Remove(previousBelow); //remove below sprite from current tile
//				//////////////////////////////////////////////////////////////////////////////

//				//get random sprite from possible sprite
//				Sprite newSprite = possibleSprite[Random.Range(0, possibleSprite.Count)];
//				newTile.GetComponent<SpriteRenderer>().sprite = newSprite;

//				//Store left col for next check
//				previousLeftCol[row] = newSprite;
//				previousBelow = newSprite;

//				newTile.GetComponent<Tile>().SetIdx(row, col);
//			}

//			//Reset previous below if one col finished
//			previousBelow = null;
//		}
		
//	}

//	public IEnumerator FindNullTiles()
//	{
//		for (int col = 0; col < gridCol; col++)
//		{
//			for (int row = 0; row < gridRow; row++)
//			{
//				if (tiles[col, row].GetComponent<SpriteRenderer>().sprite == null)
//				{
//					yield return StartCoroutine(ShiftTilesDown(col, row));
//					break;
//				}
//			}
//		}
//	}

//	private IEnumerator ShiftTilesDown(int col, int rowStart, float shiftDelay = .03f)
//	{
//		IsShifting = true;
//		List<SpriteRenderer> renders = new List<SpriteRenderer>();
//		int nullCount = 0;

//		for (int row = rowStart; row < gridRow; row++)
//		{  
//			SpriteRenderer render = tiles[col, row].GetComponent<SpriteRenderer>();
//			if (render.sprite == null)
//			{ 
//				nullCount++;
//			}
//			renders.Add(render);
//		}

//		for (int i = 0; i < nullCount; i++)
//		{ 
//			yield return new WaitForSeconds(shiftDelay);
//			for (int k = 0; k < renders.Count - 1; k++)
//			{ 
//				renders[k].sprite = renders[k + 1].sprite;
//				renders[k + 1].sprite = null; // 6
//			}
//		}
//		IsShifting = false;
//	}
//}
