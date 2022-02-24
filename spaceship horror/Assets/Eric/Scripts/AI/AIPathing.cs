using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIPathing : MonoBehaviour
{
    [SerializeField]
    public bool pauseMovement = false;

    [HideInInspector]
    public bool isPathing = true;

    List<AIPath> paths;
    NavMeshAgent navMeshAgent;

    public bool hasDestination { get { return (currentDestination != Vector3.zero); } }
    [HideInInspector]
    public Vector3 currentDestination = Vector3.zero;


    AIPath currentPath;
    Transform currentTarget;

    int currentPathIndex = 0;
    bool reversePath = false;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        paths = new List<AIPath>();

        foreach (AIPathCreator _path in FindObjectsOfType(typeof(AIPathCreator))) {
            paths.Add(_path.path);
        }
    }

    void Update()
    {
        if(pauseMovement) { navMeshAgent.destination = transform.position; return; }


        if (currentTarget != null) {
            FollowTarget();
        }

        if (currentDestination != Vector3.zero) {
            navMeshAgent.destination = currentDestination;
            if ((transform.position - currentDestination).sqrMagnitude < 2f) {
                currentDestination = Vector3.zero;  
            }
            return;
        }

        if(isPathing)
            FollowPath();
    }

    void FollowPath()
    {
        if (currentPath == null) { LookForClosestPath(); }
        navMeshAgent.destination = currentPath[currentPathIndex];

        if ((transform.position - currentPath[currentPathIndex]).sqrMagnitude < 2f) {
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
        currentDestination = Vector3.zero;
    }

    public void SetTarget(Vector3 target)
    {
        currentPath = null;
        currentTarget = null;
        currentDestination = new Vector3(target.x, transform.position.y, target.z);
    }

    public void SetSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }


    void FollowTarget()
    {
        currentDestination = currentTarget.position;
    }


}
