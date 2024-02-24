using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject[] roomPrefabs; // Assign in Inspector
    public int NumofRooms;
    private GameObject currentRoom;
    public int currentRoomIndex;

    private int[] RoomArray;

    private static int[] GenerateRoomArray(int N, int num_rooms)
    {
        if(num_rooms < 1)
        {
            throw new ArgumentException("num_rooms must be at least 1", nameof(num_rooms));
        }
        System.Random random = new System.Random();
        int[] rooms = new int[N];
        for(int i = 0; i < N; i++)
        {
            rooms[i] = random.Next(0, num_rooms);
        }

        return rooms;
    }


    private void Start()
    {
        RoomArray = GenerateRoomArray(NumofRooms, roomPrefabs.Length);
        currentRoomIndex = 0;
        ChangeRoom(0);
    }
    

    public void ChangeRoom(int roomIndex)
    {
        // Destroy the current room if it exists
        // Currently destroying all furniture for testing, but needs to be removed bc furniture data is not stored
        if (currentRoom != null)
            {
                GameObject[] allFurniture = GameObject.FindGameObjectsWithTag("Furniture");
                foreach (GameObject obj in allFurniture)
                {
                    Destroy(obj);
                }
                Destroy(currentRoom);
            }
        // Instantiate the new room
        currentRoom = Instantiate(roomPrefabs[RoomArray[roomIndex]], Vector3.zero, Quaternion.identity);
        Transform doorTransform = currentRoom.transform.Find("Doors/Right Door");
        Vector2 entrancePosition = doorTransform.position;
        // Move the player to the entrance of the new room
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = entrancePosition;
    }
}
