using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float patrolSpeed = 5f;
    public float chaseSpeed = 8f;
    public float viewRadius = 10f;
    [Range(0, 360)] public float viewAngle = 45f;
    public float attackRange = 2f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public ScreenFade screenFade;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Transform player;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public IEnemyState currentState;

    private AudioSource footstepAudio;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        footstepAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        TransitionToState(new Patrol());
    }

    void Update()
    {
        currentState.Execute();
    }

    public void TransitionToState(IEnemyState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter(this);
    }

    public bool PlayerInView()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController == null) return false;

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
                    if (!playerController.isHidden)
                    {
                        player = targetTransform;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool PlayerInAttackRange()
    {
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null && !playerController.isHidden)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                return distanceToPlayer <= attackRange;
            }
        }
        return false;
    }

    public void PlayFootStep()
    {
        if (!footstepAudio.isPlaying) footstepAudio.Play();
    }

    public void StopFootStep()
    {
        if (footstepAudio.isPlaying) footstepAudio.Stop();
    }

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

    public void InvestigateLocation(Vector3 location)
    {
        TransitionToState(new Investigate(location));
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public void TriggerGameOver()
    {
        // Hentikan pergerakan enemy
        agent.isStopped = true;

        // Hentikan pergerakan player
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SetMovable(false);
        }

        screenFade.FadeToBlack();
    }
}
