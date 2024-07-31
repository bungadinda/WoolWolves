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
        Debug.Log("patrol");
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

    public void RunAway(Vector3 location, float distance)
    {
        isRunAway = true;
        Vector3 directionAway = transform.position - location; // Menghitung arah menjauh dari lokasi
        Vector3 newDestination = transform.position + directionAway * distance; // Menghitung tujuan baru (menjauh)

        
        // Memastikan tujuan baru berada pada NavMesh
        
            agent.speed = 20f;
            agent.SetDestination(newDestination); // Menetapkan tujuan baru pada NavMeshAgent
            Debug.Log("lawrii");
            isRunAway = true;

            Invoke("ResumePatrol", 3f);
        
    }

    void ResumePatrol()
    {
        isRunAway = false;
        Debug.Log("Back to patrol again");
    }
}
