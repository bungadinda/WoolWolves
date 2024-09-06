using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints; // Titik-titik patrol
    public float patrolSpeed = 5f; // Kecepatan patrol
    public float chaseSpeed = 8f; // Kecepatan mengejar
    public float viewRadius = 10f; // Jarak pandang
    [Range(0, 360)] public float viewAngle = 45f; // Sudut pandang
    public float attackRange = 2f; // Jarak serangan
    public LayerMask targetMask; // Layer untuk mendeteksi player
    public LayerMask obstacleMask; // Layer untuk mendeteksi halangan pandangan
    public ScreenFade screenFade; // Referensi untuk screen fade
    public int meshResolution = 10; // Resolusi view cone
    public Gameplay gameplay; // Referensi ke script Gameplay

    [HideInInspector] public Animator animator;
    [HideInInspector] public Transform player; // Referensi ke player
    [HideInInspector] public NavMeshAgent agent; // NavMeshAgent untuk pergerakan
    [HideInInspector] public IEnemyState currentState; // State saat ini

    private AudioSource footstepAudio; // Audio source untuk langkah kaki
    private MeshFilter viewMeshFilter; // Filter untuk menggambar cone view
    private Mesh viewMesh; // Mesh untuk menggambar cone view

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
        viewMeshFilter = GetComponentInChildren<MeshFilter>(); // Inisialisasi MeshFilter untuk ViewCone
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        TransitionToState(new Patrol()); // Mulai di state patrol
    }

    void Update()
    {
        currentState.Execute(); // Jalankan state saat ini
    }

    void LateUpdate()
    {
        DrawFieldOfView(); // Menggambar ViewCone setiap frame
    }

    // Transisi antara state
    public void TransitionToState(IEnemyState newState)
    {
        if (currentState != null)
            currentState.Exit(); // Keluar dari state sebelumnya

        currentState = newState;
        currentState.Enter(this); // Masuk ke state baru
    }

    // Memeriksa apakah player terlihat oleh enemy
    public bool PlayerInView()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController == null || playerController.isHidden) return false; // Jika player tersembunyi, tidak terlihat

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
                    if (!playerController.isHidden) // Jika player tidak bersembunyi
                    {
                        player = targetTransform;
                        return true; // Player terlihat
                    }
                }
            }
        }
        return false;
    }

    // Memeriksa apakah player berada dalam jarak serangan
    public bool PlayerInAttackRange()
    {
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null && !playerController.isHidden)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                return distanceToPlayer <= attackRange; // Jika player dalam jarak serangan
            }
        }
        return false;
    }

    // Fungsi untuk menjalankan audio langkah kaki
    public void PlayFootStep()
    {
        if (!footstepAudio.isPlaying) footstepAudio.Play();
    }

    // Fungsi untuk menghentikan audio langkah kaki
    public void StopFootStep()
    {
        if (footstepAudio.isPlaying) footstepAudio.Stop();
    }

    // Fungsi untuk menggambar Field of View
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

    // Fungsi untuk menghitung arah dari sudut
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    Vector3 ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return hit.point; // Jika terkena halangan
        }
        else
        {
            return transform.position + dir * viewRadius; // Jika tidak ada halangan
        }
    }

    // Fungsi untuk memicu game over
    public void TriggerGameOver()
    {
        agent.isStopped = true; // Hentikan musuh

        // Hentikan pergerakan player
        PlayerController playerController = player.GetComponent
            <PlayerController>();
        if (playerController != null)
        {
            playerController.SetMovable(false); // Hentikan pergerakan player
        }

        // Trigger GameOver di gameplay
        gameplay.GameOver();
    }

    // Fungsi untuk investigasi lokasi
    public void InvestigateLocation(Vector3 location)
    {
        TransitionToState(new Investigate(location)); // Ganti state ke Investigate
    }

    // Fungsi untuk mengejar player
    public void ChasePlayer(Transform playerTransform)
    {
        PlayerController playerController = playerTransform.GetComponent<PlayerController>();
        if (playerController != null && !playerController.isHidden) // Jika player tidak bersembunyi
        {
            player = playerTransform;
            TransitionToState(new Chase()); // Ganti state ke Chase
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius); // Gambar radius pandangan

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius); // Gambar sudut pandangan
    }
}
