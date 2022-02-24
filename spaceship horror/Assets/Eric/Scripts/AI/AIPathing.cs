using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIPathing : MonoBehaviour
{
    [SerializeField]
    public bool pauseMovement = false;
    
    public bool isPathing = true;

    List<AIPath> paths;
    NavMeshAgent navMeshAgent;

    public bool hasDestination { get { return (currentDestination != Vector3.zero); } }
    [HideInInspector]
    public Vector3 currentDestination = Vector3.zero;
    List<Vector3> destinationList = new List<Vector3>();

    AIPath currentPath;
    Transform currentTarget;

    int currentPathIndex = 0;
    bool reversePath = false;

    
    public delegate void onReachedDestination();
    public event onReachedDestination reachedDestinationEvent;


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
        if(pauseMovement) { navMeshAgent.destination = transform.position; currentPath = null; return; }


        if (currentTarget != null) {
            FollowTarget();
        }

        if (currentDestination != Vector3.zero) {
            navMeshAgent.destination = currentDestination;
            if ((transform.position - new Vector3(currentDestination.x, transform.position.y, currentDestination.z)).sqrMagnitude < 2f) {

                if(destinationList.Count == 0 && reachedDestinationEvent != null) { 
                    destinationList.Clear();  
                    reachedDestinationEvent.Invoke();
                    reachedDestinationEvent = null;
                }

                if(destinationList.Count > 0) {
                    currentDestination = destinationList[0];
                    destinationList.RemoveAt(0);
                }
                else
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

        if ((transform.position - new Vector3(currentPath[currentPathIndex].x, transform.position.y, currentPath[currentPathIndex].z)).sqrMagnitude < 2f) {
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
        currentDestination = new Vector3(target.x, target.y, target.z);
    }

    public void SetMultipleTarget(List<Vector3> targets, onReachedDestination reachedDestination)
    {
        currentDestination = targets[0];
        targets.RemoveAt(0);
        destinationList = targets;

        reachedDestinationEvent += reachedDestination;
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
