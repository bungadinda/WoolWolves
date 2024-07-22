using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float patrolSpeed = 5f;   // Kecepatan patrol
    public float chaseSpeed = 8f;    // Kecepatan chase
    public float viewRadius = 10f;   // Radius pandangan
    [Range(0, 360)]
    public float viewAngle = 45f;    // Sudut pandang
    public float attackRange = 2f;   // Jarak serangan
    public LayerMask targetMask;     // Layer untuk target (player)
    public LayerMask obstacleMask;
    [HideInInspector] public Animator animator;   // Layer untuk obstacles (tembok, dll)

    [HideInInspector] public Transform player;         // Referensi ke player
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public IEnemyState currentState; // State saat ini

    private AudioSource footstepAudio;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        footstepAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        TransitionToState(new Patrol());
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Execute();
    }

    public void TransitionToState(IEnemyState newState)
    {
        if (currentState != null)
            currentState.Exit(); // Keluar dari state saat ini

        currentState = newState;
        currentState.Enter(this); // Memasuki state baru
    }

    public bool PlayerInView()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (Collider target in targetsInViewRadius)
        {
            Transform targetTransform = target.transform;
            Vector3 directionToTarget = (targetTransform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    player = targetTransform;
                    return true;
                }
            }
        }
        return false;
    }

    

    // Mengecek apakah player berada dalam jangkauan serangan
    public bool PlayerInAttackRange()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            return distanceToPlayer <= attackRange;
        }
        return false;
    }

    // buat audio play dan stop
    public void PlayFootStep()
    {
        if(!footstepAudio.isPlaying) footstepAudio.Play();
    }

    public void StopFootStep()
    {
        if(footstepAudio.isPlaying) footstepAudio.Stop();
    }

    // Menggambar field of view di Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);
    }

    // Menghitung arah dari sudut pandang
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

}
