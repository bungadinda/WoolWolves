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
    private bool setWalkPoint;
    private float range = 20f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Patrol();
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
}
