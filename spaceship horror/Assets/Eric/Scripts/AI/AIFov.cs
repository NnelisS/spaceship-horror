using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFov : MonoBehaviour
{
    [Header("Fov")]
    [Range(0, 360)]public float angle = 45;
    public float radius = 10;
    public float innerRadius = 5;
    public LayerMask obstacleMask;

    public bool TargetInView(Transform target)
    {
        float dist = Vector3.Distance(transform.position, target.position);
        if(dist > radius) { return false; }
        if(dist < innerRadius) { return true; }

        Vector3 dir = (target.position - transform.position).normalized;
        if(Vector3.Angle(transform.forward, dir) < angle / 2) {
            if(!Physics.Raycast(transform.position, dir, dist, obstacleMask)) {
                return true;
            }
        }

        return false;
    }

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

}
