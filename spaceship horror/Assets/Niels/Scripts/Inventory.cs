using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    public List<GameObject> objectives = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            objectives[i].SetActive(true);
        }
    }

    private void Update()
    {
        //Debug.Log(items.Count);
        if (items.Count >= 5)
        {
            SceneManager.LoadScene("YouWin");
        }
    }

    public void UpdateUI()
    {
        for (int i = objectives.Count - items.Count; i < objectives.Count; i++)
        {
            objectives[i].SetActive(false);
        }
    }
}