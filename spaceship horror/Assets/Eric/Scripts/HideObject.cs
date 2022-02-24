using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
    [SerializeField] Vector3 _cameraPos;
    BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }



    public Vector3 cameraPos { get { return transform.position + _cameraPos; } }

    


    public bool hidingInside = false;
    public bool openObject()
    {
        return hidingInside;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(cameraPos, Vector3.one);

    }

}
