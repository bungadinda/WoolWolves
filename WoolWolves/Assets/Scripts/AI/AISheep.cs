using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISheep : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] LayerMask groundLayer, playerLayer;
    // patrolling variables
    private Vector3 destination;
    public bool setWalkPoint;
    bool isRunAway;
    
    private float range = 20f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(!isRunAway)
        {
            Patrol(); // Panggil fungsi Patrol
        }
    }

    public void Patrol()
    {
        if(!setWalkPoint) SearchDestination();
        if(setWalkPoint) agent.SetDestination(destination);
        if(Vector3.Distance(transform.position, destination) < 10) setWalkPoint = false;
    }

    

    public void SearchDestination()
    {
        float x = Random.Range(-range, range);
        float z = Random.Range(-range, range);
        destination = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if(Physics.Raycast(destination, Vector3.down, groundLayer))
        {
            setWalkPoint = true;
        }
    }

    public void RunAway(Vector3 destination, float speed)
    {
        if (agent.isOnNavMesh && agent.isActiveAndEnabled)
        {
            agent.speed = speed;
            agent.SetDestination(destination);
        }
        else
        {
            Debug.LogWarning("Cannot set destination. NavMeshAgent is either not active or not on NavMesh.");
        }
    }

    void ResumePatrol()
    {
        isRunAway = false;
        Debug.Log("Back to patrol again");
    }
}
