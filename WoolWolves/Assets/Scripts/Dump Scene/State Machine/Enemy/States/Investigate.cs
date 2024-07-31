using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investigate : IEnemyState
{
    private EnemyAI enemy;
    public Vector3 investigateLocation;
    private float investigateSpeed = 7f;

    public Investigate(Vector3 location)
    {
        investigateLocation = location;
    }

    public void Enter(EnemyAI enemy)
    {
        this.enemy = enemy;
        enemy.animator.SetBool("isRun", true);
        enemy.agent.speed = investigateSpeed;
        enemy.PlayFootStep();
        Debug.Log("Investigate");
        enemy.agent.SetDestination(investigateLocation);
    }

    public void Execute()
    {
        if (enemy.PlayerInView())
        {
            enemy.TransitionToState(new Chase());
            return;
        }

        if (!enemy.agent.pathPending && enemy.agent.remainingDistance < 0.5f)
        {
            enemy.TransitionToState(new Patrol());
        }
    }

    public void Exit()
    {
        
    }
}
