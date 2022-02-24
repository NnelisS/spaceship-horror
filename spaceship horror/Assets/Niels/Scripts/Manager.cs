using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Cutscene");
    }

    public void Startt()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
