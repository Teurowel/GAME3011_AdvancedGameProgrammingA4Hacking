using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Will be list of sprite numbers 
//player should match sequence

public class AnswerSequence : MonoBehaviour
{
    [SerializeField] GameObject sequenceTilePrefab;
    
    List<GameObject> sequence = new List<GameObject>();


    public void GenerateSequence(int sequenceSize, List<Sprite> tileSprites, Vector3 bufferTileSize, Vector3 bufferTileStartPos)
    {
        //Calcualte tile's start position
        Vector3 startPos = Vector3.zero;
        startPos.x = bufferTileStartPos.x;
        startPos.y = transform.position.y;

        for (int col = 0; col < sequenceSize; ++col)
        {
            //create one tile
            GameObject tileGameObject = Instantiate(sequenceTilePrefab);
            sequence.Add(tileGameObject);

            //Set the size of tile
            tileGameObject.transform.localScale = bufferTileSize;
            tileGameObject.transform.SetParent(transform);

            //SEt sprite
            int randomIdx = Random.Range(0, GridManager.instance.NumOfTileType);
            tileGameObject.GetComponent<SpriteRenderer>().sprite = tileSprites[randomIdx];

            //Set the position of tile
            Vector3 tilePos = Vector3.zero;

            tilePos.x = startPos.x + (col * bufferTileSize.x);
            tilePos.y = startPos.y;
            tilePos.z = -2.0f;

            tileGameObject.transform.position = tilePos;
        }
    }
}
