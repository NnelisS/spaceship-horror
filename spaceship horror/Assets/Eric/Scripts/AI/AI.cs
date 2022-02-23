using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIPathing), typeof(AIFov))]
public class AI : MonoBehaviour
{
    [SerializeField] PlayerController player;
    AIPathing pathing;
    AIFov fov;

    [SerializeField] States state = States.Roaming;

    [Header("Aggro Behavior")]
    public float aggroTime = 1.0f;
    [Header("SearchBehavior")]
    public float searchTime = 1.0f;
    public float searchAreaSize = 10f;

    void Awake()
    {
        pathing = GetComponent<AIPathing>();
        fov = GetComponent<AIFov>();
    }

    void Update()
    {

        if(state != States.Chasing) {
            if (fov.TargetInView(player.transform)) {
                StopAllCoroutines();
                state = States.Chasing;
                pathing.SetTarget(player.transform);
            }
        }

        if(state == States.Chasing) {
            if (!fov.TargetInView(player.transform)) {
                state = States.Searching;
                StartCoroutine(Searching(player.transform.position));
                pathing.SetTarget(null);
            }
        }

    }


    IEnumerator Searching(Vector3 searchArea)
    {
        pathing.isPathing = false;
        StartCoroutine(NewSearchPos(searchArea));
        yield return new WaitForSeconds(searchTime);
        StopAllCoroutines();
        state = States.Roaming;
        pathing.isPathing = true;
    }

    IEnumerator NewSearchPos(Vector3 searchArea)
    {
        if (pathing.hasDestination) { yield return null; }
        else { 
            yield return new WaitForSeconds(1f);
            Debug.Log(searchArea);

            pathing.SetTarget(searchArea + Random.insideUnitSphere * searchAreaSize);
        }
        StartCoroutine(NewSearchPos(searchArea));
    }




    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (state == States.Chasing) { Gizmos.DrawLine(transform.position, player.transform.position); }

        if(pathing != null)
            if (pathing.hasDestination) {
                Gizmos.DrawWireSphere(pathing.currentDestination, 1f);
            }
    }

    enum States
    {
        Roaming,
        Searching,
        Chasing
    }
}


