using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public Inventory inventory;

    private void OnMouseDown()
    {
        inventory.items.Add(this.gameObject);
        inventory.objectives.RemoveAt(0);
        Destroy(this.gameObject);
    }

    public void OnMouseOver()
    {
        Debug.Log("Hovering");
    }

    private void OnMouseExit()
    {
        Debug.Log("NotHovering");
    }
}
