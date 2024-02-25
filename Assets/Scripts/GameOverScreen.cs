using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    void Start()
    {
        HideGameOverScreen();
    }
    
    public void HideGameOverScreen()
    {
        gameObject.SetActive(false);
    }
}
