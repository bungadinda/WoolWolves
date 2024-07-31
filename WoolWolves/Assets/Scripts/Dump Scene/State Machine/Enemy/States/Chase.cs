using UnityEngine;

public class Chase : IEnemyState
{
    private float chaseDuration = 10f; // Durasi chase
    private float chaseCounter;
    private EnemyAI enemy;

    public void Enter(EnemyAI enemy)
    {
        this.enemy = enemy;
        enemy.agent.speed = enemy.chaseSpeed;
        chaseCounter = chaseDuration;
        Debug.Log("chase");
    }

    public void Execute()
    {
        
        if (enemy.PlayerInAttackRange())
        {
            
            enemy.TransitionToState(new Attack());
        }
        else
        {
            // kejar player
            enemy.agent.SetDestination(enemy.player.position);
            // durasi mengejar berkurang
            chaseCounter -= Time.deltaTime;
            if(chaseCounter <= 0)
            {
                // kalau udh 0 durasinya kembali ke patrol
                enemy.TransitionToState(new Patrol());
            } 
            
        }
    }

    public void Exit()
    {
    }
}
