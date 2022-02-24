using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class AIPath
{
    [SerializeField, HideInInspector]
    List<Vector3> points;
    [HideInInspector]
    public bool isClosed;
    [HideInInspector]
    public bool freezeY;

    public bool lockPath = false;

    public int numPoints { get { return points.Count; } }
    public Vector3 this[int i] { get { return points[LoopIndex(i)]; } }
    int LoopIndex(int i) { return (i + points.Count) % points.Count; }

    public Vector3 centroid
    {
        get {
            Vector3 centroid = new Vector3();
            for (int i = 0; i < numPoints; i++) {
                centroid += points[i];
            }

            return centroid/numPoints;
        }
    }

    public AIPath(Vector3 center)
    {
        points = new List<Vector3>()
        {
            center + Vector3.forward,
            center + Vector3.back
        };
    }

    public void AddPoint(Vector3 pos)
    {
        if (lockPath) { return; }
        points.Add(new Vector3(pos.x, 0, pos.z));
    }

    public void MovePoint(int i, Vector3 pos)
    {
        if(lockPath) { return; }
        points[i] = new Vector3(pos.x, pos.y, pos.z);
    }

    public void UpdateY(int i, float y)
    {
        points[i] = new Vector3(points[i].x, y, points[i].z);
    }

    public void ToggleClosed()
    {
        isClosed = !isClosed;
    }
}
