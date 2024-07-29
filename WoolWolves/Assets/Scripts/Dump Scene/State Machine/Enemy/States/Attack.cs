using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : IEnemyState
{
    private EnemyAI enemy;
    private float attackCooldown = 1.0f;
    private float lastAttackTime;

    public void Enter(EnemyAI enemy)
    {
        this.enemy = enemy;
        enemy.agent.isStopped = true;
        lastAttackTime = Time.time - attackCooldown;
    }

    public void Execute()
    {
        if (!enemy.PlayerInAttackRange())
        {
            enemy.TransitionToState(new Chase());
        }
        else
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
    }

    public void Exit()
    {
        enemy.agent.isStopped = false;
    }

    private void AttackPlayer()
    {
        Debug.Log("Enemy attacks player!");
        enemy.TriggerGameOver();
    }
}
