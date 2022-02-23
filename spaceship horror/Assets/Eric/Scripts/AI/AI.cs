using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIPathing), typeof(AIFov))]
public class AI : MonoBehaviour
{
    [SerializeField] Transform player;
    AIPathing pathing;
    AIFov fov;

    [Header("Aggro Behavior")]
    public float aggroTime = 1.0f;
    public bool aggro = false;

    void Awake()
    {
        pathing = GetComponent<AIPathing>();
        fov = GetComponent<AIFov>();
    }

    void Update()
    {
        if (fov.TargetInView(player) && aggro == false) {
            Aggro();
        }
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (aggro) { Gizmos.DrawLine(transform.position, player.position); }
    }


}
