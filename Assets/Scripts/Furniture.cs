using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public List<Sprite> possibleSprites; 
    private SpriteRenderer spriteRenderer;
    private int spriteIndex;

    private Transform room;
    private PopulateRoom roomScript;
    private List<GameObject> furnitureLocations;

    private bool isMonster = false;

    private PlayerController playerScript;

    private GameObject enemy;

    public void RestartCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteIndex = Random.Range(0, possibleSprites.Count);
        spriteRenderer.sprite = possibleSprites[spriteIndex];
        possibleSprites.RemoveAt(spriteIndex);

        GameObject player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
        enemy = Resources.Load<GameObject>("Mimic");
    }

    // Hiding monster
    public void Update() {
        // Monster is hiding and player started reloading
        if (isMonster && playerScript.isReloading) {
            Instantiate(enemy, new Vector3(transform.position.x, transform.position.y, -3), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    // Turns the furniture into a monster!
    public void monstrify() {
        isMonster = true;
        room = this.transform.parent;
        roomScript = room.GetComponent<PopulateRoom>();
        furnitureLocations = roomScript.furnitureLocations;
        
        // Move furniture
        if (Random.Range(0, 2) == 0 && roomScript.predeterminedFurniture.Contains(gameObject)) {
            int index = Random.Range(0, furnitureLocations.Count);
            GameObject furnitureToMonstrify = furnitureLocations[index];
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
        
        this.gameObject.tag = "Monster";
    }

    // Monster destroyed in hiding 
    public void OnDestroy() {
        playerScript.bulletsInChamber = 2;
        playerScript.reloadSound.Play();
    }
}
