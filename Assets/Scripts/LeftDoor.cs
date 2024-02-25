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
    
    private void OnTriggerEnter2D(Collider2D other)
    {   
        playerObject = GameObject.Find("Player");
        playerScript = playerObject.GetComponent<PlayerController>();
        playerMovingLeft = playerScript.movingLeft;
        if (other.CompareTag("Player") && playerMovingLeft)
        {
            Debug.Log("He cooking");
            MapController MapController = FindObjectOfType<MapController>();
            MapController.currentRoomIndex++;
            MapController.ChangeRoom();
        } 
    }
}
