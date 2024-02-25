using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BulletDisplayController : MonoBehaviour
{
    public Image imageComponent;
    public Sprite fullyLoaded;
    public Sprite halfLoaded;
    public Sprite empty;

    public PlayerController playerScript;

    void Start() {
        GameObject player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.bulletsInChamber == 2) {
            imageComponent.sprite = fullyLoaded;
        }   

        else if (playerScript.bulletsInChamber == 1) {
            imageComponent.sprite = halfLoaded;
        } 

        else {
            imageComponent.sprite = empty;
        }
    }
}
