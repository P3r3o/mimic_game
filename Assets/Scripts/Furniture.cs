using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public List<Sprite> possibleSprites; 
    private SpriteRenderer spriteRenderer;
    private int spriteIndex;

    private Transform room;
    private PopulateRoom roomScript;
    private List<GameObject> allFurnitureInRoom;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteIndex = Random.Range(0, possibleSprites.Count);
        spriteRenderer.sprite = possibleSprites[spriteIndex];
        possibleSprites.RemoveAt(spriteIndex);

        room = this.transform.parent;
        roomScript = room.GetComponent<PopulateRoom>();
        allFurnitureInRoom = roomScript.allFurnitureInRoom;
    }

    // Turns the furniture into a monster!
    public void monstrify() {
        // Move furniture
        if (false) {
            int index = Random.Range(0, allFurnitureInRoom.Count);
            GameObject furnitureToMonstrify = allFurnitureInRoom[index];
            transform.position = furnitureToMonstrify.transform.position;

            Debug.Log("Moved furniture");
        }

        // Change appearance of furniture
        else {
            int index = Random.Range(0, possibleSprites.Count);
            spriteRenderer.sprite = possibleSprites[index];
            Debug.Log("Changed appearance of sprite");
        }
    }
}
