using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AIPathing), typeof(AIFov), typeof(SphereCollider))]
public class AI : MonoBehaviour
{
    [SerializeField] PlayerController player;
    AIPathing pathing;
    AIFov fov;

    [SerializeField] States state = States.Roaming;

    [Header("Pause pathing or Movement")]
    [SerializeField] bool pauseMovement = false;
    [SerializeField] bool pausePathing = false;

    [Header("Attack Behavior")]
    [SerializeField] float attackCoolDown = 1;

    [Header("Enemy speed")]
    [SerializeField] float normalSpeed = 8;
    [SerializeField] float chasingSpeed = 10;

    [Header("Search Behavior")]
    [SerializeField] float searchTime = 1.0f;
    [SerializeField] public float searchAreaSize = 10f;


    float attackTimer = 0.0f;

    void Awake()
    {
        pathing = GetComponent<AIPathing>();
        fov = GetComponent<AIFov>();
    }

    void Start()
    {
        pathing.SetSpeed(normalSpeed);
    }

    void Update()
    {
        pathing.pauseMovement = pauseMovement;
        pathing.isPathing = !pausePathing;


        if (state != States.Chasing) {
            if (fov.TargetInView(player.transform)) {
                StopAllCoroutines();
                state = States.Chasing;
                pathing.SetSpeed(chasingSpeed);
                pathing.SetTarget(player.transform);
            }
        }

        if (state == States.Chasing) {
            if (!fov.TargetInView(player.transform)) {
                state = States.Searching;
            }
            attackTimer += Time.deltaTime;
        }

        if(state == States.Searching) {
            StartCoroutine(Searching(player.transform.position));
            pathing.SetTarget(null);
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
            pathing.SetSpeed(normalSpeed);
            yield return new WaitForSeconds(Random.Range(0.75f, 1.5f));
            pathing.SetTarget(searchArea + Random.insideUnitSphere * searchAreaSize);
        }
        StartCoroutine(NewSearchPos(searchArea));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && attackTimer >= attackCoolDown && state == States.Chasing) {
            player.health.TakeDamage(34);
            attackTimer = 0.0f;
        }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (state == States.Chasing) { Gizmos.DrawLine(transform.position, player.transform.position); }

        if (pathing != null && pathing.hasDestination)
            Gizmos.DrawWireSphere(pathing.currentDestination, 1f);
    }

    enum States
    {
        Roaming,
        Searching,
        Chasing
    }
}


