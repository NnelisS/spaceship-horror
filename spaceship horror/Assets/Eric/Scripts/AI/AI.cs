using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIPathing), typeof(AIFov), typeof(CapsuleCollider))]
public class AI : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Animator animator;

    AIPathing pathing;
    AIFov fov;
    CapsuleCollider _collider;

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
    HideObject searchingObject = null;

    float screamCoolDownTime = 10f;
    bool screamOnCoolDown = false;


    void Awake()
    {
        pathing = GetComponent<AIPathing>();
        fov = GetComponent<AIFov>();
        _collider = GetComponentInChildren<CapsuleCollider>();
    }

    void Start()
    {
        pathing.SetSpeed(normalSpeed);
    }

    void Update()
    {

        pathing.pauseMovement = pauseMovement;
        pathing.isPathing = !pausePathing;

        if(searchingObject != null) { transform.LookAt(searchingObject.transform.position); }

        if (state != States.Chasing) {
            if (fov.TargetInView(player.transform) && !player.hiding) {
                StopAllCoroutines();
                state = States.Chasing;
                animator.SetInteger("state", (int)state);
                pathing.SetSpeed(chasingSpeed);
                pathing.SetTarget(player.transform);
                searchingObject = null;
            }
            //SearchObject(fov.HideObjectInView(), 0.25f);
        }

        if (state == States.Chasing) {
            if (!fov.TargetInView(player.transform) || player.hiding) {
                state = States.Searching;
                animator.SetInteger("state", (int)state);
                pausePathing = true;

                if (fov.HideObjectInView() != null) {
                    SearchObject(fov.HideObjectInView(), 1f);
                }
                else {
                    pathing.SetTarget(player.transform.position);
                }
                StartCoroutine(Searching(player.transform.position));
            }
            attackTimer += Time.deltaTime;
        }

        if(state == States.Searching) {
            
        }

    }

    void SearchObject(HideObject _object, float chanceInProcent)
    {
        if (_object == null) { return; }
        if(Random.value > 1 - chanceInProcent) {
            state = States.Searching;
            animator.SetInteger("state", (int)state);
            pathing.SetTarget(null);
            searchingObject = _object;
            List<Vector3> targetList = new List<Vector3> { _object.transform.position + _object.transform.forward * 5 , _object.transform.position + _object.transform.forward * 4 };
            pathing.SetMultipleTarget(targetList, OpenObject);
        }

    }

    void Scream()
    {

    }

    void OpenObject()
    {
        if(searchingObject.openObject()) { player.Hide(); }
        searchingObject = null;
        StopSearching();
    }

    void StopSearching()
    {
        state = States.Roaming;
        StopAllCoroutines();
        animator.SetInteger("state", (int)state);
        pausePathing = false;
        pathing.SetSpeed(normalSpeed);
    }


    IEnumerator Searching(Vector3 searchArea)
    {
        pathing.SetSpeed(normalSpeed);
        searchArea.y = transform.position.y;
        StartCoroutine(NewSearchPos(searchArea));
        yield return new WaitForSeconds(searchTime);
        StopSearching();
    }

    IEnumerator NewSearchPos(Vector3 searchArea)
    {
        if (pathing.hasDestination) { yield return null; }
        else {
            yield return new WaitForSeconds(Random.Range(0.75f, 1f));
            SearchObject(fov.HideObjectInView(), 0.1f);

            Vector3 dir = (Random.insideUnitSphere * searchAreaSize);
            dir.y = searchArea.y;
            float dst = Vector3.Distance(searchArea, searchArea + dir.normalized);

            Debug.DrawRay(searchArea, dir * dst, Color.red, 2f);

            RaycastHit hit;
            Physics.Raycast(searchArea, dir, out hit, dst);

            if(hit.collider != null) {
                pathing.SetTarget(hit.point);
            }
            else {
                pathing.SetTarget(searchArea + dir);
            }

        }
        StartCoroutine(NewSearchPos(searchArea));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && attackTimer >= attackCoolDown && state == States.Chasing && !player.hiding) {
            player.health.TakeDamage(34);
            attackTimer = 0.0f;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (state == States.Chasing) { Gizmos.DrawLine(transform.position, player.transform.position); }

        if (pathing != null && pathing.hasDestination)
            Gizmos.DrawWireSphere(pathing.currentDestination, 3f);
    }

    enum States
    {
        Roaming,
        Searching,
        Chasing
    }
}


