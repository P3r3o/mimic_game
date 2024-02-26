using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyController : MonoBehaviour
{
    public Image imageComponent;
    public Sprite key;

    public PlayerController playerScript;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
        Color color = imageComponent.color;
        color.a = 0f;
        imageComponent.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        Color color = imageComponent.color;

        if (playerScript.killedMonsterInRoom) {
            color.a = 255f;
            imageComponent.color = color;
        } else {
            color.a = 0f;
            imageComponent.color = color;
        }
    }
}
