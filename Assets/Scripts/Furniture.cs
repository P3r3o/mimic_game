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
    private bool isHunting = false;

    private PlayerController playerScript;

    public GameObject player;
    public float rotationSpeed = 5f;
    public float chargeSpeed = 3f;
    public float chargeTime = 2f;
    public float waitTime = 3f;

    private float chargeTimer;
    private float waitTimer;
    private bool isCharging;

    public void RestartCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    void Start() {
        chargeTimer = chargeTime;
        waitTimer = waitTime;
        isCharging = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteIndex = Random.Range(0, possibleSprites.Count);
        spriteRenderer.sprite = possibleSprites[spriteIndex];
        possibleSprites.RemoveAt(spriteIndex);

        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    // Hunting
    public void Update() {
        // Monster is hiding and player started reloading
        if (isMonster && !isHunting && playerScript.isReloading) {
            isHunting = true;
            playerScript.killedMonsterInRoom = true;

            // Monster transformation
            // TODO: Assign animator to furniture from monster prefab
            GameObject prefabWithAnimator = Resources.Load<GameObject>("mimic");
            Animator prefabAnimator = prefabWithAnimator.GetComponent<Animator>();

            // Assuming this script is attached to the GameObject that needs to change its animation
            Animator thisAnimator = GetComponent<Animator>();

            // Copy the Animator Controller from the prefab to this GameObject
            if (prefabAnimator != null && thisAnimator != null) {
                thisAnimator.runtimeAnimatorController = prefabAnimator.runtimeAnimatorController;
                thisAnimator.SetBool("isMimic", true);
            }
            // Close doors

            Debug.Log("THE HUNT IS ON!!!");
        } 

        // Hunting
        if (isMonster && isHunting) {
            // Rotate towards the player
            Vector2 direction = player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (isCharging)
            {
                // Charge towards the player
                transform.position += transform.right * chargeSpeed * Time.deltaTime;
                chargeTimer -= Time.deltaTime;

                if (chargeTimer <= 0)
                {
                    // Stop charging and start waiting
                    isCharging = false;
                    waitTimer = waitTime;
                }
            }

            else
            {
                // Wait for the next charge
                waitTimer -= Time.deltaTime;

                if (waitTimer <= 0)
                {
                    // Start charging
                    isCharging = true;
                    chargeTimer = chargeTime;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
{
    if (other.gameObject.CompareTag("Player") && isHunting && isMonster) {
        StartCoroutine(DeathAndRestart(other.transform));
    }
}

private IEnumerator DeathAndRestart(Transform player)
{
    Animator playerAnimator = player.GetComponent<Animator>();
    playerAnimator.SetTrigger("dead");
    Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.velocity = Vector2.zero; // Stop any current movement immediately
        rb.isKinematic = true; // Prevent the Rigidbody from being affected by physics
    }
    yield return new WaitForSeconds(2);
    rb.isKinematic = false;
    RestartCurrentScene();
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
}
