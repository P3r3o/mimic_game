using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateRoom : MonoBehaviour
{
    private static Transform possibleFurnitureLocations;
    public GameObject[] possibleFurnitureTypes;
    public List<GameObject> furnitureLocations;
    public List<GameObject> allFurnitureInRoom;
    public List<GameObject> predeterminedFurniture;
    public int minimumNumberOfFurniture = 2;
    public int maximumNumberOfFurniture = 4;

    void Start()
    {
        possibleFurnitureLocations = transform.Find("Furniture");
        furnitureLocations = new List<GameObject>();
        allFurnitureInRoom = new List<GameObject>();

        for (int i = 0; i < possibleFurnitureLocations.childCount; i++)
        {
            furnitureLocations.Add(possibleFurnitureLocations.GetChild(i).gameObject);
        }

        int currentNumberOfFurniture = predeterminedFurniture.Count;
        minimumNumberOfFurniture = Random.Range(minimumNumberOfFurniture, maximumNumberOfFurniture);

        while (currentNumberOfFurniture < minimumNumberOfFurniture) {
            int locationIndex = Random.Range(0, furnitureLocations.Count);
            int furnitureIndex = Random.Range(0, possibleFurnitureTypes.Length);
            
            GameObject furniture = furnitureLocations[locationIndex];
            GameObject newFurniture = Instantiate(possibleFurnitureTypes[furnitureIndex], new Vector3(furniture.transform.position.x, furniture.transform.position.y, -2), Quaternion.identity);
            newFurniture.transform.SetParent(gameObject.transform, false);
            allFurnitureInRoom.Add(newFurniture);

            furnitureLocations.RemoveAt(locationIndex);
            currentNumberOfFurniture++;
        }

        allFurnitureInRoom.AddRange(predeterminedFurniture);
        Debug.Log(allFurnitureInRoom.Count);
    }
}
