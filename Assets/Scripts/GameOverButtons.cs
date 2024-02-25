using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{
    public void MainMenu() {
        SceneManager.LoadScene(0);
    }

    public void Exit() {
        Application.Quit();
    }
}
