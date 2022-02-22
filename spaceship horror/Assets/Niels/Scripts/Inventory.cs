using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    public List<GameObject> objectives = new List<GameObject>();

    public int currentCount;

    private void Start()
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            objectives[i].SetActive(true);
        }
    }

    private void Update()
    {
        currentCount = items.Count;
        
        Debug.Log(items.Count);
    }
}