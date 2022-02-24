using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Color defaultColor;
    [SerializeField] Color hoverColor;
    [SerializeField] Text interactText;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Camera playerCam;

    void Update()
    {
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
 
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10f, layerMask)) {
            if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Interactable") {
                image.color = hoverColor;
                interactText.enabled = true;
            }
        }
        else {
            image.color = defaultColor;
            interactText.enabled = false;
        }
    }


}
