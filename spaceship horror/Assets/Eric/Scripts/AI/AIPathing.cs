using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPathing : MonoBehaviour
{
    [SerializeField] List<AIPath> paths;

    NavMeshAgent navMeshAgent;
    AIPath currentPath;
    Transform currentTarget;
    int currentPathIndex = 0;
    public bool reversePath = false;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        foreach (AIPathCreator _path in FindObjectsOfType(typeof(AIPathCreator))) {
            paths.Add(_path.path);
        }

    }

    void Update()
    {
        if (currentTarget != null) {
            FollowTarget();
        }

        else {
            FollowPath();
        }
    }

    void FollowPath()
    {
        if (currentPath == null) { LookForClosestPath(); return; }
        navMeshAgent.destination = currentPath[currentPathIndex];

        if ((transform.position - currentPath[currentPathIndex]).sqrMagnitude < 1f) {
            currentPathIndex += reversePath ? -1 : 1;

            if (reversePath ? currentPathIndex < 0 : currentPathIndex >= currentPath.numPoints) {
                LookForClosestPath();
            }
        }
    }

    void LookForClosestPath()
    {
        AIPath closest = currentPath;
        int closestIndex = 0;

        foreach (AIPath path in paths) {
            if (path == currentPath) { continue; }
            if (closest == currentPath) { closest = path; continue; }

            if ((transform.position - path.centroid).sqrMagnitude < (transform.position - closest.centroid).sqrMagnitude) {
                closest = path;
            }
        }

        for (int i = 1; i < closest.numPoints; i++) {
            if ((transform.position - closest[i]).sqrMagnitude < (transform.position - closest[closestIndex]).sqrMagnitude) {
                closestIndex = i;
            }
        }

        reversePath = (closestIndex >= closest.numPoints / 2) ? true : false;

        currentPathIndex = closestIndex;
        currentPath = closest;
    }

    public void SetTarget(Transform target)
    {
        currentPath = null;
        currentTarget = target;

        LookForClosestPath();


    }

    void FollowTarget()
    {
        navMeshAgent.destination = currentTarget.position;
    }


}
