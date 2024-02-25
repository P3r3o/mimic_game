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
    private List<GameObject> furnitureChildren;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteIndex = Random.Range(0, possibleSprites.Count);
        spriteRenderer.sprite = possibleSprites[spriteIndex];
        possibleSprites.RemoveAt(spriteIndex);
    }

    // Turns the furniture into a monster!
    public void monstrify() {
        room = this.transform.parent;
        roomScript = room.GetComponent<PopulateRoom>();
        furnitureChildren = roomScript.furnitureChildren;
        
        // Move furniture
        if (Random.Range(0, 2) == 0) {
            int index = Random.Range(0, furnitureChildren.Count);
            GameObject furnitureToMonstrify = furnitureChildren[index];
            transform.position = furnitureToMonstrify.transform.position;

            // change tag of furniture to monster
            furnitureToMonstrify.gameObject.tag = "Monster";

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
