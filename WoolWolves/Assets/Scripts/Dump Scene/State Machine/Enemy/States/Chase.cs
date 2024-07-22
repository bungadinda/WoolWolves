using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : IEnemyState
{
    private EnemyAI enemy;
    private float chaseDuration = 10f; // Durasi chase
    private float chaseCounter;
    
    public void Enter(EnemyAI enemy)
    {
        this.enemy = enemy;
        enemy.agent.speed = enemy.chaseSpeed;
        chaseCounter = chaseDuration;
        Debug.Log("Chase State");
        enemy.animator.SetBool("isRun", true);
        enemy.PlayFootStep();
    }

    public void Execute()
    {
        // if player in attack range
        if(enemy.PlayerInAttackRange()) enemy.TransitionToState(new Attack());
        else if(enemy.PlayerInView())
        {
            chaseCounter = chaseDuration;
            enemy.agent.SetDestination(enemy.player.position);
        }
        else
        {
            chaseCounter -= Time.deltaTime;
            if(chaseCounter <= 0)
            {
                enemy.animator.SetBool("isRun", false);
                enemy.TransitionToState(new Patrol());
            } 
        }
    }

    public void Exit()
    {
        enemy.StopFootStep();
    }

    
}
