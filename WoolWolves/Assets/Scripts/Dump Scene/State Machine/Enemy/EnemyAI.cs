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
    private MeshFilter viewMeshFilter;
    private Mesh viewMesh;
    public Gameplay gameplay;
    public int meshResolution = 10;

    void Awake()
    {
        gameplay = GameObject.FindObjectOfType<Gameplay>();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        footstepAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewMeshFilter = GetComponentInChildren<MeshFilter>(); // Inisialisasi MeshFilter
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        TransitionToState(new Patrol());
    }

    void Update()
    {
        currentState.Execute();
    }

    void LateUpdate()
    {
        DrawFieldOfView(); // Panggil fungsi untuk menggambar ViewCone setiap frame
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
        if (playerController == null || playerController.isHidden) return false;

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

    public void ChasePlayer(Transform playerTransform)
    {
        PlayerController playerController = playerTransform.GetComponent<PlayerController>();
        if (playerController != null && !playerController.isHidden)
        {
            player = playerTransform;
            TransitionToState(new Chase());
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    // Fungsi untuk menggambar ViewCone
    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            viewPoints.Add(ViewCast(angle));
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    Vector3 ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return hit.point;
        }
        else
        {
            return transform.position + dir * viewRadius;
        }
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
        // Trigger ke gameplay untuk menjalankan fungsi GameOver
        gameplay.GameOver();
    }
}
