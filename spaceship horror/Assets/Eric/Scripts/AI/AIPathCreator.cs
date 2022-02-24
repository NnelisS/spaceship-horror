using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathCreator : MonoBehaviour
{
    [SerializeField]
    float groundHeight = 10;

    [HideInInspector]
    public AIPath path;

    void Start()
    {
        //ResetY();
        UpdatePoints();
    }

    public void CreatePath()
    {
        path = new AIPath(transform.position);
    }

    public void ResetY()
    {
        for (int i = 0; i < path.numPoints; i++) {
            path.UpdateY(i, groundHeight);
        }
    }

    public void UpdatePoints()
    {
        for (int i = 0; i < path.numPoints; i++) {
            RaycastHit hit;
            Physics.Raycast(path[i], Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"));

            if(hit.point != null)
                path.UpdateY(i, hit.point.y);
        }
    }


}
