using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIFov : MonoBehaviour
{
    [Header("Fov")]
    [Range(0, 360)] public float angle = 45;
    public float radius = 10;
    public float innerRadius = 5;
    public LayerMask obstacleMask;
    public List<HideObject> hideObjects;

    void Start()
    {
        foreach (HideObject _object in FindObjectsOfType(typeof(HideObject))) {
            hideObjects.Add(_object);
        }
    }


    public bool TargetInView(Transform target)
    {
        float dist = Vector3.Distance(transform.position, target.position);
        if(dist > radius) { return false; }

        Vector3 dir = (target.position - transform.position).normalized;
        if(Vector3.Angle(transform.forward, dir) < angle / 2 || dist < innerRadius) {
            if(!Physics.Raycast(transform.position, dir, dist, obstacleMask)) {
                return true;
            }
        }

        return false;
    }

    public HideObject HideObjectInView()
    {
        foreach(HideObject _object in hideObjects) {
            float dist = Vector3.Distance(transform.position, _object.transform.position);
            if (dist < radius) { return _object; }
        }

        return null;
    }


    public Vector3 DirFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

}
