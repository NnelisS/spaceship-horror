using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public Inventory inventory;

    private void OnMouseDown()
    {
        inventory.items.Add(this.gameObject);
        inventory.UpdateUI();
        gameObject.SetActive(false);
    }

    public void OnMouseOver()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(255, 255, 255);
        //Debug.Log("Hovering");
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0);
        //Debug.Log("NotHovering");
    }
}
