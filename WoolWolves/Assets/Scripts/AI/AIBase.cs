using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBase : MonoBehaviour
{
    // ai sight
    public bool playerInSight = false;
    public float fieldOfViewAngle = 160f;
    public float losRadius = 45f;

    // ai sight and memorizing the player
    private bool isMemorizePlayer = false;
    public float memoryStarTime = 10f;
    private float increasingMemoryTime;

    // ai hearing
    private Vector3 noisePosition;
    private bool aiHeardPlayer = false;
    public float noiseTravelDistance = 50f;
    public float spinSpeed = 3f;
    private bool canSpin = false;
    private float isSpinningTime;
    public float spinTime = 3f;

    // patrolling randomly between waypoints
    public Transform[] moveSpots;
    private int randomSpot;

    // wait time when finish at moveSpots
    private float waitTime;
    public float startWaitTime = 1f;

    private NavMeshAgent nav;

    // ai strafe
    public float disttoPlayer = 5f;
    private float randomStrafeStartTime;
    private float waitStrafeTime;
    public float t_minStrafe;
    public float t_maxStrafe;

    public Transform strafeRight;
    public Transform strafeLeft;
    private int randomStrafeDir;

    // ai chase
    public float chaseRadius = 20f;
    public float facePlayerFactor = 20f;

    

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.enabled = true;
    }

    void Start()
    {
        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);
        randomStrafeDir = Random.Range(0, 2);
    }

    void Update()
    {
        float distance = Vector3.Distance(PlayerMovement.playerPos, transform.position);
        if(distance <= losRadius)
        {
            // checking line of sight
            CheckLOS();
        }

        if(nav.isActiveAndEnabled)
        {
            // check all conditions
            CheckAgent();
        }
    }

    void LateUpdate()
    {
        if(isMemorizePlayer && !playerInSight) disttoPlayer = 0.5f;
        else disttoPlayer = 10f;
    }

    public void CheckLOS()
    {
        Vector3 direction = PlayerMovement.playerPos - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        if(angle < fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, direction.normalized, out hit, losRadius))
            {
                if(hit.collider.tag == "Player")
                {
                    playerInSight = true;
                    isMemorizePlayer = true;
                }
                else
                {
                    playerInSight = false;
                }
            }
        }
    }

    public void CheckAgent()
    {
        if(!playerInSight && !isMemorizePlayer && !aiHeardPlayer)
        {
            // patrolling, check noise
            Patrol(); CheckNoise();
            StopCoroutine(AiMemory());
        }
        else if(aiHeardPlayer && !playerInSight && !isMemorizePlayer)
        {
            canSpin = true;
            GoToNoisePosition();
        }
        else if(playerInSight)
        {
            isMemorizePlayer = true;
            FacePlayer();
            ChasePlayer();
        }
        else if(isMemorizePlayer && !playerInSight)
        {
            ChasePlayer();
            StartCoroutine(AiMemory());
        }
    }

    private void ChasePlayer()
    {
        float distance = Vector3.Distance(PlayerMovement.playerPos, transform.position);
        if(distance <= chaseRadius && distance > disttoPlayer) {nav.SetDestination(PlayerMovement.playerPos); nav.speed = 5f;}
        else if(nav.isActiveAndEnabled && distance <= disttoPlayer)
        {
            randomStrafeDir = Random.Range(0, 2);
            randomStrafeStartTime = Random.Range(t_minStrafe, t_maxStrafe);
            if(waitStrafeTime <= 0)
            {
                if(randomStrafeDir == 0) nav.SetDestination(strafeLeft.position);
                else if(randomStrafeDir == 1) nav.SetDestination(strafeRight.position);
                waitStrafeTime = randomStrafeStartTime;
                
            }
            else waitStrafeTime -= Time.deltaTime;
        }
        
    }

    private void FacePlayer()
    {
        Vector3 direction = (PlayerMovement.playerPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * facePlayerFactor);
    }

    public void Patrol()
    {
        nav.SetDestination(moveSpots[randomSpot].position);
        if(Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 2.0f)
        {
            if(waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
            }
            else waitTime -= Time.deltaTime;
        }
    }

    public void CheckNoise()
    {

    }

    public void GoToNoisePosition()
    {
        nav.SetDestination(noisePosition);
        float distance = Vector3.Distance(transform.position, noisePosition);
        if(distance <= 5f && canSpin)
        {
            isSpinningTime += Time.deltaTime;
            transform.Rotate(Vector3.up * spinSpeed, Space.World);
            if(isSpinningTime >= spinTime)
            {
                canSpin = false;
                aiHeardPlayer = false;
                isSpinningTime = 0f;
            }
        }
    }

    IEnumerator AiMemory()
    {
        increasingMemoryTime = 0;
        while(increasingMemoryTime < memoryStarTime)
        {
            increasingMemoryTime += Time.deltaTime;
            isMemorizePlayer = true;
            yield return null;
        }
        aiHeardPlayer = false;
        isMemorizePlayer = false;
    }

}
