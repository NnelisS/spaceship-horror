using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathCreator : MonoBehaviour
{
    [HideInInspector]
    public AIPath path;

    public void CreatePath()
    {
        path = new AIPath(transform.position);
    }

}
