using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MapController : MonoBehaviour
{
    private GameObject playerObject;
    public Sprite[] slides;
    public float slideDuration = 1f;
    private Image displayImage;
    private PlayerController playerScript;
    private bool playerMovingLeft;

    public GameObject[] roomPrefabs; 
    public int numOfRooms;
    private GameObject currentRoom;
    private List<GameObject> visitedRooms = new List<GameObject>();
    public int currentRoomIndex;
    private int[] roomArray;

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
        GameObject.Find("Map Controller/Canvas/Panel").SetActive(false);
        roomArray = GenerateRoomArray(numOfRooms, roomPrefabs.Length);
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
            if (playerMovingLeft) {
                currentRoom.SetActive(false);
            } else {
                Destroy(currentRoom);
            }
        } 

        // Entering a new room
        if (currentRoomIndex == -1) {
            //victory
            Debug.Log("WINNER");
            GameObject.Find("Map Controller/Canvas/Panel").SetActive(true);
            GameObject.Find("Canvas").SetActive(false);

            displayImage = GetComponentInChildren<Image>();
            StartCoroutine(PlaySlideshow());
        }
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
            PopulateRoom roomScript = currentRoom.GetComponent<PopulateRoom>();
            List<GameObject> allFurniture = roomScript.allFurnitureInRoom;
            int index = UnityEngine.Random.Range(0, allFurniture.Count);
            GameObject monsterFurniture = allFurniture[index];
            Furniture monstrifyScript = monsterFurniture.GetComponent<Furniture>();
            monstrifyScript.monstrify();
        } 


        Vector2 entrancePosition = doorTransform.position;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float offset = 0.5f;

        if (playerMovingLeft) {
            offset = -0.5f;
        }

        player.transform.position = new Vector2(entrancePosition.x + offset, entrancePosition.y);
        playerScript.doorOpen.Play();
    }
    private IEnumerator PlaySlideshow()
    {
        foreach (var slide in slides)
        {
            Debug.Log("SLIDESHOW");
            displayImage.sprite = slide; // Set the current slide
            yield return new WaitForSeconds(slideDuration * 0.2f); // Wait for the slide duration
        }
        SceneManager.LoadScene(0);
        // Optionally, load a new scene or trigger other actions after the slideshow
        // For example: SceneManager.LoadScene("NextSceneName");
    }
}
