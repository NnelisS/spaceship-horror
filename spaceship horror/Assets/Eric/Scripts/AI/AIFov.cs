using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFov : MonoBehaviour
{
    [Header("Fov")]
    [Range(0, 360)]public float angle = 45;
    public float radius = 10;
    public LayerMask obstacleMask;

    public bool TargetInView(Transform target)
    {
        float dist = Vector3.Distance(transform.position, target.position);
        if(dist > radius) { return false; }

        Vector3 dir = (target.position - transform.position).normalized;
        if(Vector3.Angle(transform.forward, dir) < angle / 2) {
            if(!Physics.Raycast(transform.position, dir, dist, obstacleMask)) {
                return true;
            }
        }


        return false;
    }

    Vector3 DirFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 viewAngleA = DirFromAngle(-angle / 2);
        Vector3 viewAngleB = DirFromAngle(angle / 2);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * radius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * radius);
    }


}
