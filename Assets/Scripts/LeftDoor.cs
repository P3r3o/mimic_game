using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftDoor : MonoBehaviour
{
    public int roomIndexToLoad;
    public Vector3 spawnPosition;
    private GameObject playerObject;
    private PlayerController playerScript;
    private bool playerMovingLeft;
    
    private void OnCollisionEnter2D(Collision2D other)
    {   
        playerObject = GameObject.Find("Player");
        playerScript = playerObject.GetComponent<PlayerController>();
        playerMovingLeft = playerScript.movingLeft;
        
        if (other.gameObject.CompareTag("Player") && playerMovingLeft)
        {
            MapController MapController = FindObjectOfType<MapController>();
            MapController.currentRoomIndex++;
            MapController.ChangeRoom();
        } 
    }
}
