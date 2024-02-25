using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private GameObject playerObject;
    private PlayerController playerScript;
    private bool playerMovingLeft;

    public GameObject[] roomPrefabs; // Assign in Inspector
    public int numOfRooms;
    private GameObject currentRoom;
    private List<GameObject> visitedRooms = new List<GameObject>();
    public int currentRoomIndex;

    private int[] RoomArray;

    private static int[] GenerateRoomArray(int numOfRooms, int numOfRoomTypes)
    {
        if(numOfRoomTypes < 1)
        {
            throw new ArgumentException("numOfRoomTypes must be at least 1", nameof(numOfRoomTypes));
        }

        System.Random random = new System.Random();
        int[] rooms = new int[numOfRooms];

        for(int i = 0; i < numOfRooms; i++)
        {
            // Set the final room 
            if (i == numOfRooms - 1) {
                rooms[i] = numOfRoomTypes - 1;
                break;
            } 

            rooms[i] = random.Next(0, numOfRoomTypes - 1);
        }

        return rooms;
    }


    private void Start()
    {
        RoomArray = GenerateRoomArray(numOfRooms, roomPrefabs.Length);
        Debug.Log(RoomArray.Length);
        currentRoomIndex = 0;
        ChangeRoom();
    }
    

    public void ChangeRoom()
    {
        playerObject = GameObject.Find("Player");
        playerScript = playerObject.GetComponent<PlayerController>();
        playerMovingLeft = playerScript.movingLeft;

        // Set everything in the current room to inactive
        if (currentRoom != null)
        {
            GameObject[] allFurniture = GameObject.FindGameObjectsWithTag("Furniture");
            foreach (GameObject obj in allFurniture)
            {
                obj.SetActive(false);
            }
            currentRoom.SetActive(false);
            Debug.Log("Neutered room");
        }

        // Entering a new room
        if (currentRoomIndex >= visitedRooms.Count) {
            Debug.Log("Added room");
            currentRoom = Instantiate(roomPrefabs[RoomArray[currentRoomIndex]], Vector3.zero, Quaternion.identity);
            visitedRooms.Add(currentRoom);
        } 
        
        // Visiting a previously visited room
        else {
            currentRoom = visitedRooms[currentRoomIndex];
            currentRoom.SetActive(true);
            
            // Loop through all the children of the currentRoom
            foreach (Transform child in currentRoom.transform)
            {
                Debug.Log("mruh");
                // Set each child game object to active
                child.gameObject.SetActive(true);
            }
        }
        
        
        Transform doorTransform;

        // Moving to the left 
        if (playerMovingLeft) {
            doorTransform = currentRoom.transform.Find("Doors/Right Door");
        }

        // Moving to the right
        else {
            doorTransform = currentRoom.transform.Find("Doors/Left Door");
        } 


        Vector2 entrancePosition = doorTransform.position;
        // Move the player to the entrance of the new room
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = entrancePosition;
    }
}
