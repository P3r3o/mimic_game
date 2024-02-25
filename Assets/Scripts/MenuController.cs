using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public TMP_Text instructionText;

    public void Start() {
        instructionText.enabled = false;
    }

    public void PlayGame() {
        SceneManager.LoadScene(1);
    }
    public void Instructions() {
        instructionText.enabled = true;
    }
    public void Exit() {
        Application.Quit();
    }
}
