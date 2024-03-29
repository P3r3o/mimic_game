using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightDoor : MonoBehaviour
{
    public int roomIndexToLoad;
    public Vector3 spawnPosition;
    private GameObject playerObject;
    private PlayerController playerScript;
    private bool playerMovingRight;

    private void OnCollisionEnter2D(Collision2D other)
    {   
        playerObject = GameObject.Find("Player");
        playerScript = playerObject.GetComponent<PlayerController>();
        playerMovingRight = !playerScript.movingLeft;

        // Collision with player
        if (other.gameObject.CompareTag("Player") && playerMovingRight) {
            // You can only leave the room once the monster is killed
            if (playerScript.killedMonsterInRoom || playerScript.justGotBaby)
            {
                MapController MapController = FindObjectOfType<MapController>();
                MapController.currentRoomIndex--;
                MapController.ChangeRoom();
                playerScript.justGotBaby = false;
                playerScript.killedMonsterInRoom = false;
            } 
        }
    }
}
