using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private GameObject playerObject;
    private PlayerController playerScript;
    private bool playerMovingLeft;

    public GameObject[] roomPrefabs; 
    public int numOfRooms;
    private GameObject currentRoom;
    private List<GameObject> visitedRooms = new List<GameObject>();
    public int currentRoomIndex;
    private int[] roomArray;

    private bool[] monsterArray;
    public int minimumNumberOfMonsters;
    public int maximumNumberOfMonsters;

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

    private static bool[] GenerateMonsterArray(int numOfRooms, int minimumNumberOfMonsters, int maximumNumberOfMonsters) 
    {
        bool[] monsters = new bool[numOfRooms];
        List<int> roomsWithoutMonsters = new List<int>();

        for (int i = 0; i < numOfRooms; i++) {
            roomsWithoutMonsters.Add(i);
            monsters[i] = false;
        }

        int currNumOfMonsters = 0;
        minimumNumberOfMonsters = UnityEngine.Random.Range(minimumNumberOfMonsters, maximumNumberOfMonsters);

        while (currNumOfMonsters < minimumNumberOfMonsters) {
            int index = UnityEngine.Random.Range(0, roomsWithoutMonsters.Count - 1);
            monsters[index] = true;
            currNumOfMonsters++;
            roomsWithoutMonsters.RemoveAt(index);
        }


        return monsters;
    }


    private void Start()
    {
        roomArray = GenerateRoomArray(numOfRooms, roomPrefabs.Length);
        monsterArray = GenerateMonsterArray(numOfRooms, minimumNumberOfMonsters, maximumNumberOfMonsters);
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
            currentRoom.SetActive(false);
        }

        // Entering a new room
        if (currentRoomIndex >= visitedRooms.Count) {
            currentRoom = Instantiate(roomPrefabs[roomArray[currentRoomIndex]], Vector3.zero, Quaternion.identity);
            visitedRooms.Add(currentRoom);
        } 
        
        // Visiting a previously visited room
        else {
            currentRoom = visitedRooms[currentRoomIndex];
            currentRoom.SetActive(true);
        }
        
        
        Transform doorTransform;

        // Moving to the left 
        if (playerMovingLeft) {
            doorTransform = currentRoom.transform.Find("Doors/Right Door");
        }

        // Moving to the right
        else {
            doorTransform = currentRoom.transform.Find("Doors/Left Door");

            // If we're walking back into a room with a monster, then spawn a monster!
            if (monsterArray[currentRoomIndex]) {
                PopulateRoom roomScript = currentRoom.GetComponent<PopulateRoom>();
                List<GameObject> allFurniture = roomScript.allFurnitureInRoom;
                int index = UnityEngine.Random.Range(0, allFurniture.Count);
                GameObject monsterFurniture = allFurniture[index];
                Furniture monstrifyScript = monsterFurniture.GetComponent<Furniture>();
                Debug.Log(monsterFurniture);
                Debug.Log(monstrifyScript);
                
                monstrifyScript.monstrify();
            }
        } 


        Vector2 entrancePosition = doorTransform.position;
        // Move the player to the entrance of the new room
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = entrancePosition;
    }
}
