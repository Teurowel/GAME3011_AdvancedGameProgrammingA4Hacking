using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Will be list of sprite numbers 
//player should match sequence

public class AnswerSequence : MonoBehaviour
{
    public enum ESequenceStatus
    {
        Progress,
        Succeeded,
        Failed
    }

    [SerializeField] GameObject sequenceTilePrefab;
    
    List<GameObject> sequence = new List<GameObject>();
    int sequenceSize = 0;
    int sequenceCheckIdx = 0;
    int sequenceScore = 0;
    Vector3 sequenceTileStartPos = Vector3.zero;

    ESequenceStatus status = ESequenceStatus.Progress;

    public void GenerateSequence(int _sequenceSize, List<Sprite> tileSprites, Vector3 bufferTileSize, Vector3 bufferTileStartPos)
    {
        //Calcualte tile's start position
        sequenceTileStartPos = Vector3.zero;
        sequenceTileStartPos.x = bufferTileStartPos.x;
        sequenceTileStartPos.y = transform.position.y;

        sequenceSize = _sequenceSize;

        for (int col = 0; col < _sequenceSize; ++col)
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

            tilePos.x = sequenceTileStartPos.x + (col * bufferTileSize.x);
            tilePos.y = sequenceTileStartPos.y;
            tilePos.z = -2.0f;

            tileGameObject.transform.position = tilePos;
        }

        sequenceScore = sequenceSize * 50;
    }

    public void CheckSequence(Sprite bufferSprite, int bufferCheckIdx)
    {
        //If this sequence is not progress...
        if(status != ESequenceStatus.Progress)
        {
            return;
        }

        //If sequence have same sprite
        if (sequence[sequenceCheckIdx].GetComponent<SpriteRenderer>().sprite == bufferSprite)
        {
            ++sequenceCheckIdx;

            //If buffer and sequence matched... succedd
            if (sequenceCheckIdx == sequenceSize)
            {
                Debug.Log("Match succeeded");
                status = ESequenceStatus.Succeeded;
                GetComponent<SpriteRenderer>().color = Color.green;
                GlobalData.instance.ModifyScore(sequenceScore);

                GridManager.instance.IncreaseNumOfCompletedAnswerSequence();
            }
        }
        //If sequence have different sprite, move sequence back
        else
        {
            //If we can't move back furthre... failed
            if(bufferCheckIdx + sequenceSize + 1 > GridManager.instance.ListOfBuffer.Count)
            {
                Debug.Log("Match failed");
                status = ESequenceStatus.Failed;
                GetComponent<SpriteRenderer>().color = Color.red;
                GridManager.instance.IncreaseNumOfCompletedAnswerSequence();
                return;
            }

            sequenceCheckIdx = 0;

            for(int i = 0; i < sequence.Count; ++i)
            { 
                //Set the position of tile
                Vector3 tilePos = Vector3.zero;

                tilePos.x = sequenceTileStartPos.x + ((bufferCheckIdx + 1 + i) * GridManager.instance.BufferTileSize.x);
                tilePos.y = sequenceTileStartPos.y;
                tilePos.z = -2.0f;

                sequence[i].transform.position = tilePos;
            }
        }
    }
}
