using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    public Tilemap tilemap; // Assign this in the inspector
    public Tile wallTile; // Assign a wall tile in the inspector
    public int roomSize = 10; // Size of the room

    // Start is called before the first frame update
    void Start()
    {
        GenerateRoom(roomSize);
    }

    void GenerateRoom(int size)
    {
        // Generate floor
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                // Check if we're on the edge to place a wall
                if (x == 0 || y == 0 || x == size - 1 || y == size - 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
