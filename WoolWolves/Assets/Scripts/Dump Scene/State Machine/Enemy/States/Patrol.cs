using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : IEnemyState
{
    private EnemyAI enemy;
    private float waitTime = 2f; // Waktu menunggu saat rotating
    private float waitCounter;
    private bool waiting;

    public void Enter(EnemyAI enemy)
    {
        this.enemy = enemy;
        enemy.agent.speed = enemy.patrolSpeed;
        enemy.StartCoroutine(StartPatrol());
        enemy.PlayFootStep();
    }

    public void Execute()
    {
        // if the player can viewed by enemy, then switch to chase state
        if(enemy.PlayerInView()) { enemy.TransitionToState(new Chase()); }
        else if(waiting) 
        {
            enemy.animator.SetBool("isWalk", false);
            enemy.transform.Rotate(0, 45 * Time.deltaTime, 0);
            Debug.Log("rotating");
            waitCounter -= Time.deltaTime;
            if(waitCounter <= 0)
            {
                waiting = false;
                enemy.StartCoroutine(StartPatrol());
            }
        }
    }

    public void Exit()
    {
        enemy.StopAllCoroutines();
        enemy.StopFootStep();
    }

    private IEnumerator StartPatrol()
    {
        while(true) 
        {
            Debug.Log("patrol");
            Vector3 patrolPoint = enemy.patrolPoints[Random.Range(0, enemy.patrolPoints.Length)].position;
            enemy.agent.SetDestination(patrolPoint);
            enemy.animator.SetBool("isWalk", true);
            while(enemy.agent.remainingDistance > enemy.agent.stoppingDistance)
            {
                Debug.Log("finish point");
                yield return null; // waiting 'til the enemy at the patrol point
            }

            waiting = true;
            waitCounter = waitTime;
            yield break;

        }
    }
}
