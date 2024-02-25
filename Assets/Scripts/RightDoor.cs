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

    private void OnTriggerEnter2D(Collider2D other)
    {   
        playerObject = GameObject.Find("Player");
        playerScript = playerObject.GetComponent<PlayerController>();
        playerMovingRight = !playerScript.movingLeft;
        if (other.CompareTag("Player") && playerMovingRight)
        {
            MapController MapController = FindObjectOfType<MapController>();
            MapController.currentRoomIndex--;
            MapController.ChangeRoom();
        } 
    }
}
