using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickManager : MonoBehaviour
{
    public float time;
    public float timer = 36.50f;

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = time;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SceneManager.LoadScene("Level 1");
        }
    }
}
