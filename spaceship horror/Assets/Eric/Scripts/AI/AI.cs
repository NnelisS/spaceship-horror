using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIPathing))]
public class AI : MonoBehaviour
{
    [SerializeField] Transform player;
    AIPathing pathing;

    [Header("Aggro Behavior")]
    public float aggroRadius = 10f;
    public float aggroTime = 1.0f;
    public bool aggro = false;

    void Awake()
    {
        pathing = GetComponent<AIPathing>();
    }

    void Update()
    {
        //if((player.position - transform.position).magnitude < aggroRadius && aggro == false) {
        //    Aggro();
        //}

    }

    void Aggro()
    {
        aggro = true;
        pathing.SetTarget(player);
        StartCoroutine(AggroRoutine());
    }

    IEnumerator AggroRoutine()
    {
        yield return new WaitForSeconds(aggroTime);
        aggro = false;
        pathing.SetTarget(null);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
        
    }

}
