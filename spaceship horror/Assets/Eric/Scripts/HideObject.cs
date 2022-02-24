using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
    [SerializeField] Vector3 _cameraPos;
    public Vector3 cameraPos { get { return transform.position.normalized + _cameraPos; } }

    public bool hidingInside = false;

    public void enemyFoundPlayer()
    {

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(cameraPos, Vector3.one);

    }

}
