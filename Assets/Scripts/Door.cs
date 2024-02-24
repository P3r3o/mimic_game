using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int roomIndexToLoad;
    public Vector3 spawnPosition;
    
    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag("Player"))
        {
            Debug.Log("He cooking");
            MapController MapController = FindObjectOfType<MapController>();
            MapController.currentRoomIndex++;
            MapController.ChangeRoom(MapController.currentRoomIndex);
        }
    }
}
