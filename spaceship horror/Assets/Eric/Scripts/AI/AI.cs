using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AIPathing), typeof(AIFov), typeof(SphereCollider))]
public class AI : MonoBehaviour
{
    [SerializeField] PlayerController player;
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


        if (state != States.Chasing) {
            if (fov.TargetInView(player.transform) && !player.hiding) {
                StopAllCoroutines();
                state = States.Chasing;
                pathing.SetSpeed(chasingSpeed);
                pathing.SetTarget(player.transform);
            }
            //SearchObject(fov.HideObjectInView(), 0.25f);
        }

        if (state == States.Chasing) {
            if (!fov.TargetInView(player.transform) || player.hiding) {
                state = States.Searching;
                pausePathing = true;

                if (fov.HideObjectInView().hidingInside) {
                    SearchObject(fov.HideObjectInView(), 1f);
                    Debug.Log("?");
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
        if(_object == null) { return; }
        if(Random.value > 1 - chanceInProcent) {
            pathing.SetTarget( _object.transform.position + _object.transform.forward * 2);



        }

    }


    void StopSearching()
    {
        state = States.Roaming;
        pausePathing = false;
        pathing.SetSpeed(normalSpeed);
    }

    IEnumerator Searching(Vector3 searchArea)
    {
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
            SearchObject(fov.HideObjectInView(), 0.25f);

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


