using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateRoom : MonoBehaviour
{
    private static Transform possibleFurnitureLocations;
    public GameObject[] possibleFurnitureTypes;
    public List<GameObject> furnitureChildren;
    public List<GameObject> allFurnitureInRoom;
    public List<GameObject> predeterminedFurniture;
    public int minimumNumberOfFurniture = 2;
    public int maximumNumberOfFurniture = 4;

    void Start()
    {
        possibleFurnitureLocations = transform.Find("Furniture");
        furnitureChildren = new List<GameObject>();
        allFurnitureInRoom = new List<GameObject>();

        for (int i = 0; i < possibleFurnitureLocations.childCount; i++)
        {
            furnitureChildren.Add(possibleFurnitureLocations.GetChild(i).gameObject);
        }

        int currentNumberOfFurniture = 0;
        minimumNumberOfFurniture = Random.Range(minimumNumberOfFurniture, maximumNumberOfFurniture);

        while (currentNumberOfFurniture < minimumNumberOfFurniture) {
            int locationIndex = Random.Range(0, furnitureChildren.Count);
            int furnitureIndex = Random.Range(0, possibleFurnitureTypes.Length);
            
            GameObject furniture = furnitureChildren[locationIndex];
            GameObject newFurniture = Instantiate(possibleFurnitureTypes[furnitureIndex], new Vector3(furniture.transform.position.x, furniture.transform.position.y, -1), Quaternion.identity);
            newFurniture.transform.SetParent(gameObject.transform, false);
            allFurnitureInRoom.Add(newFurniture);

            furnitureChildren.RemoveAt(locationIndex);
            currentNumberOfFurniture++;
        }

        allFurnitureInRoom.AddRange(predeterminedFurniture);
    }
}
